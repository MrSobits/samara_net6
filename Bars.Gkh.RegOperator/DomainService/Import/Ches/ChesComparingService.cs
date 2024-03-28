namespace Bars.Gkh.RegOperator.DomainService.Import.Ches
{
    using System.Linq;

    using AutoMapper;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Caching.LinqExtensions;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Utils;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService.AddressMatching;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Import.Ches;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Imports.Ches;
    using Bars.Gkh.RegOperator.Imports.Ches.PreImport;

    using Castle.Windsor;

    using Npgsql;

    using EnumerableExtensions = NHibernate.Util.EnumerableExtensions;

    /// <summary>
    /// Сервис для работы с сопоставлением данных во время импорта ЧЭС
    /// </summary>
    public class ChesComparingService : IChesComparingService
    {
        private SchemaQualifiedObjectName AddressMatchTableName => new SchemaQualifiedObjectName
        {
            Name = "CHES_NOT_MATCH_ADDRESS",
            Schema = "IMPORT"
        };
        private SchemaQualifiedObjectName AccountOwnerNotMatchTableName => new SchemaQualifiedObjectName
        {
            Name = "CHES_NOT_MATCH_ACC_OWNER",
            Schema = "IMPORT"
        };
        private SchemaQualifiedObjectName LegalAccountOwnerNotMatchTableName => new SchemaQualifiedObjectName
        {
            Name = "CHES_NOT_MATCH_LEGAL_ACC_OWNER",
            Schema = "IMPORT"
        };
        private SchemaQualifiedObjectName IndividualAccountOwnerNotMatchTableName => new SchemaQualifiedObjectName
        {
            Name = "CHES_NOT_MATCH_IND_ACC_OWNER",
            Schema = "IMPORT"
        };

        public ISessionProvider SessionProvider { get; set; }
        public IWindsorContainer Container { get; set; }
        
        public IMapper Mapper { get; set; }
        public IChesImportService ChesImportService { get; set; }
        public IAddressMatcher AddressMatcher { get; set; }
        public IChesAccountOwnerComparingService AccountOwnerComparingService { get; set; }
        public IDomainService<ChesNotMatchAddress> ChesNotMatchAddressDomain { get; set; }
        public IDomainService<ChesMatchAccountOwner> ChesMatchAccountOwnerDomain { get; set; }
        public IDomainService<ChesNotMatchAccountOwner> ChesNotMatchAccountOwnerDomain { get; set; }
        public IDomainService<ChesNotMatchLegalAccountOwner> ChesNotMatchLegalAccountOwnerDomain { get; set; }
        public IDomainService<PersonalAccountOwner> PersonalAccountOwnerDomain { get; set; }

        /// <inheritdoc />
        public IDataResult ProcessAccountImported(IChesTempDataProvider chesTempDataProvider)
        {
            ArgumentChecker.IsType<AccountFileInfo>(chesTempDataProvider.FileInfo, "importer.FileInfo");

            try
            {
                this.Container.InTransaction(() =>
                {
                    this.ProcessAddressMatching(chesTempDataProvider);
                    this.ProcessAccountOwnerMatching(chesTempDataProvider);
                });

                return new BaseDataResult();
            }
            catch (NpgsqlException exception)
            {
                return BaseDataResult.Error(exception.Message);
            }
        }

        /// <inheritdoc />
        public IDataResult ProcessAddressMatchAdded(params AddressMatch[] address)
        {
            if (address.Any())
            {
                EnumerableExtensions.ForEach(address.Split(1000),
                    portion =>
                {
                    this.SessionProvider.GetCurrentSession().CreateSQLQuery($"DELETE FROM {this.AddressMatchTableName} WHERE EXTERNAL_ADDRESS in "
                        + $"({string.Join(",", portion.Select(x => $"'{x.ExternalAddress}'").ToArray())})").ExecuteUpdate();
                });
            }

            return new BaseDataResult();
        }

        /// <inheritdoc />
        public IDataResult ProcessAddressMatchRemoved(AddressMatch address)
        {
            this.ChesNotMatchAddressDomain.Save(new ChesNotMatchAddress
            {
                ExternalAddress = address.ExternalAddress,
                HouseGuid = address.HouseGuid
            });

            return new BaseDataResult();
        }

        /// <inheritdoc />
        public IDataResult ProcessOwnerMatchAdded(ChesMatchAccountOwner owner)
        {
            this.SessionProvider.GetCurrentSession().Delete("from ChesNotMatchAccountOwner o where "
                + $"o.Name = '{owner.Name}' and "
                + $"o.PersonalAccountNumber = '{owner.PersonalAccountNumber}' and "
                + $"o.OwnerType = {(int)owner.OwnerType}");

            return new BaseDataResult();
        }

        /// <inheritdoc />
        public IDataResult ProcessOwnerMatchRemoved(ChesMatchAccountOwner owner)
        {
            this.ChesNotMatchAccountOwnerDomain.Save(Mapper.Map<ChesMatchAccountOwner, ChesNotMatchAccountOwner>(owner));
            return new BaseDataResult();
        }

        /// <inheritdoc />
        public IDataResult MatchOwner(BaseParams baseParams)
        {
            var notMatchId = baseParams.Params.GetAsId("notMatchId");
            var ownerId = baseParams.Params.GetAsId("ownerId");

            var externalOwner = this.ChesNotMatchAccountOwnerDomain.Get(notMatchId);
            var owner = this.PersonalAccountOwnerDomain.Get(ownerId);
            if (externalOwner.IsNull() || owner.IsNull())
            {
                return BaseDataResult.Error("Не найден абонент для сопоставления");
            }

            var legalOwner = externalOwner as ChesNotMatchLegalAccountOwner;

            // если физик, то сопоставляем 1:1, если юрик, то всех с совпадающими именем, инн, кпп
            var ownersToMatch = externalOwner.OwnerType == PersonalAccountOwnerType.Individual
                ? new[] { externalOwner }
                : this.ChesNotMatchLegalAccountOwnerDomain.GetAll()
                    .WhereIf(legalOwner.Inn.Length != 12, x => x.Kpp == legalOwner.Kpp )
                    .Where(x => x.Inn == legalOwner.Inn && x.Name == legalOwner.Name)
                  
                    .ToArray();

            this.Container.InStatelessTransaction(session =>
            {
                foreach (var chesNotMatchAccountOwner in ownersToMatch)
                {
                    var chesMatchOwner = Mapper.Map<ChesNotMatchAccountOwner, ChesMatchAccountOwner>(chesNotMatchAccountOwner);
                    chesMatchOwner.AccountOwner = owner;

                    session.InsertOrUpdate(chesMatchOwner);
                }

                var ids = ownersToMatch.Select(x => x.Id).ToArray();

                session
                    .CreateSQLQuery($"DELETE FROM {this.LegalAccountOwnerNotMatchTableName} where id in (:ids)")
                    .SetParameterList("ids", ids)
                    .ExecuteUpdate();

                session
                    .CreateSQLQuery($"DELETE FROM {this.IndividualAccountOwnerNotMatchTableName} where id in (:ids)")
                    .SetParameterList("ids", ids)
                    .ExecuteUpdate();

                session
                    .CreateSQLQuery($"DELETE FROM {this.AccountOwnerNotMatchTableName} where id in (:ids)")
                    .SetParameterList("ids", ids)
                    .ExecuteUpdate();
            });

            return new BaseDataResult();
        }

        private void ProcessAddressMatching(IChesTempDataProvider chesTempDataProvider)
        {
            var tableName = chesTempDataProvider.TableName;
            var session = this.SessionProvider.GetCurrentSession();

            // чистим несопоставленные за период
            var query = $"delete from {this.AddressMatchTableName} where PERIOD_ID = {chesTempDataProvider.Period.Id}";
            session.CreateSQLQuery(query).ExecuteUpdate();

            // добавляем импортированные
            query = $@"
                        insert into {this.AddressMatchTableName} (EXTERNAL_ADDRESS, HOUSE_GUID, PERIOD_ID) 
                            select distinct on (acc.ADDHOUSE) acc.ADDHOUSE, acc.FIASHOUSEID, {chesTempDataProvider.Period.Id} from {tableName} acc
                                where not exists(select null from GKH_ADDRESS_MATCH address where ADDRESS.EXTERNAL_ADDRESS = ACC.ADDHOUSE)";

            session.CreateSQLQuery(query).ExecuteUpdate();

            var addresses = this.ChesNotMatchAddressDomain.GetAll()
                .Where(x => x.Period == chesTempDataProvider.Period)
                .Select(x => new AddressMatchDto
                {
                    Address = x.ExternalAddress,
                    HouseGuid = x.HouseGuid
                })
                .ToArray();

            var result = this.AddressMatcher.MatchAddresses(addresses);
            this.ProcessAddressMatchAdded(result.Data.ToArray());
        }

        private void ProcessAccountOwnerMatching(IChesTempDataProvider chesTempDataProvider)
        {
            var tableName = chesTempDataProvider.TableName;
            var session = this.SessionProvider.GetCurrentSession();

            // чистим несопоставленные за период
            var query = $@"with q as (select id from {this.AccountOwnerNotMatchTableName} where PERIOD_ID = {chesTempDataProvider.Period.Id})
                delete from {this.LegalAccountOwnerNotMatchTableName} o where exists (select null from q where q.id = o.id)";
            session.CreateSQLQuery(query).ExecuteUpdate();

            query = $@"with q as (select id from {this.AccountOwnerNotMatchTableName} where PERIOD_ID = {chesTempDataProvider.Period.Id})
                delete from {this.IndividualAccountOwnerNotMatchTableName} o where exists (select null from q where q.id = o.id)";
            session.CreateSQLQuery(query).ExecuteUpdate();

            query = $"delete from {this.AccountOwnerNotMatchTableName} where PERIOD_ID = {chesTempDataProvider.Period.Id}";
            session.CreateSQLQuery(query).ExecuteUpdate();

            var tempTableName = $"TEMP_NOT_MATCH_ACCOUNT_{chesTempDataProvider.Period.Id}";

            query = $@"DROP TABLE IF EXISTS {tempTableName}";
            session.CreateSQLQuery(query).ExecuteUpdate();

            query = $@"CREATE TEMP TABLE {tempTableName} (
                id BIGINT DEFAULT nextval('import.ches_not_match_acc_owner_id_seq'), -- подставляем id из секвенкции
                OWNER_TYPE      integer NOT NULL,
                PERS_ACC_NUM    character varying(20) NOT NULL,
                NAME            varchar(300),
                SURNAME         varchar(100),
                FIRSTNAME       varchar(100),
                LASTNAME        varchar(100),
                BIRTH_DATE      timestamp without time zone,
                INN             varchar(100),
                KPP             varchar(100))";
            session.CreateSQLQuery(query).ExecuteUpdate();

            // добавляем импортированных физиков
            query = $@"insert into {tempTableName} (OWNER_TYPE, PERS_ACC_NUM, NAME, SURNAME, FIRSTNAME, LASTNAME, BIRTH_DATE)
                        select 
                                BILLTYPE,
                                LSNUM, 
                                trim(coalesce(SURNAME, '') || ' ' || coalesce(NAME, '') || ' ' || coalesce(LASTNAME, '')),
                                SURNAME, 
                                NAME, 
                                LASTNAME, 
                                BIRTHDATE
                        from {tableName} acc
                        where BILLTYPE = {(int)PersonalAccountOwnerType.Individual} and
                            (acc.NAME is not null) and
                            not exists(
                                select null 
                                from IMPORT.CHES_MATCH_IND_ACC_OWNER owner 
                                join IMPORT.CHES_MATCH_ACC_OWNER bowner on bowner.id = owner.id
                                where owner.SURNAME = acc.SURNAME and owner.FIRSTNAME = acc.NAME and owner.LASTNAME = acc.LASTNAME 
                                    and (acc.BIRTHDATE = owner.BIRTH_DATE 
                                        or acc.BIRTHDATE is null and owner.BIRTH_DATE is null))";

            session.CreateSQLQuery(query).ExecuteUpdate();

            // добавляем импортированных юриков
            query = $@"insert into {tempTableName} (OWNER_TYPE, PERS_ACC_NUM, NAME, INN, KPP)
                        select 
                                BILLTYPE,
                                LSNUM, 
                                trim(RENTERNAME),
                                trim(INN), 
                                trim(KPP)
                            from {tableName} acc
                            where BILLTYPE = {(int)PersonalAccountOwnerType.Legal} and 
                            (acc.INN is not null and acc.RENTERNAME is not null) and
                                not exists(
                                    select null 
                                    from IMPORT.CHES_MATCH_LEGAL_ACC_OWNER owner 
                                    join IMPORT.CHES_MATCH_ACC_OWNER bowner on bowner.id = owner.id
                                    where owner.INN = trim(acc.INN) and  owner.KPP = trim(acc.KPP) and bowner.NAME = trim(acc.RENTERNAME))";

            session.CreateSQLQuery(query).ExecuteUpdate();

            query = $@"insert into {this.AccountOwnerNotMatchTableName} (ID, NAME, OWNER_TYPE, PERS_ACC_NUM, PERIOD_ID)
                        select ID, NAME, OWNER_TYPE, PERS_ACC_NUM, {chesTempDataProvider.Period.Id} from {tempTableName}";
            session.CreateSQLQuery(query).ExecuteUpdate();

            query = $@"INSERT INTO {this.IndividualAccountOwnerNotMatchTableName} (ID, SURNAME, FIRSTNAME, LASTNAME, BIRTH_DATE)
                        select ID, SURNAME, FIRSTNAME, LASTNAME, BIRTH_DATE from {tempTableName} where OWNER_TYPE = {(int)PersonalAccountOwnerType.Individual}";
            session.CreateSQLQuery(query).ExecuteUpdate();

            query = $@"INSERT INTO {this.LegalAccountOwnerNotMatchTableName} (ID, INN, KPP)
                        select ID, INN, KPP from {tempTableName} where OWNER_TYPE = {(int)PersonalAccountOwnerType.Legal}";
            session.CreateSQLQuery(query).ExecuteUpdate();

            query = $@"DROP TABLE IF EXISTS {tempTableName}";
            session.CreateSQLQuery(query).ExecuteUpdate();

            this.AccountOwnerComparingService
                .MatchAutomatically(this.ChesNotMatchAccountOwnerDomain.GetAll().Where(x => x.Period == chesTempDataProvider.Period))
                .Data
                .ForEach(x => this.ProcessOwnerMatchAdded(x));
        }
    }
}