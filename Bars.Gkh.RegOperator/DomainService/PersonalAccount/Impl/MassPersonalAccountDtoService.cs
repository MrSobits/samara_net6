namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using AutoMapper;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Dto;

    using Castle.Windsor;

    using Dapper;

    using NHibernate;

    /// <summary>
    /// Сервис массовой работы с <see cref="BasePersonalAccountDto"/>
    /// <para>Использовать при массовых импортах ЛС</para>
    /// </summary>
    /// <remarks>
    /// Этапы работы с сервисом:
    /// 1. Инициализируем кэш (InitCache)
    /// 2. Добавляем изменения ЛС (AddPersonalAccount)
    /// 3. Применяем изменения (ApplyChanges)
    /// </remarks>
    public class MassPersonalAccountDtoService : IMassPersonalAccountDtoService
    {
        private static readonly object SyncRoot = new object();

        private readonly IDomainService<BasePersonalAccountDto> basePersonalAccountDtoDomain;
        private readonly IWindsorContainer container;
        private readonly IMapper mapper;
        private readonly ISessionProvider sessionProvider;

        private long maxIdExistsPersonalAccount;
        private HashSet<BasePersonalAccount> accounts;
        private bool useStatelessSession;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="container">Контейнер</param>
        /// <param name="sessionProvider">Фабрика сессий</param>
        /// <param name="basePersonalAccountDtoDomain">Домен-сервис <see cref="BasePersonalAccountDto"/></param>
        public MassPersonalAccountDtoService(
            IWindsorContainer container,
            ISessionProvider sessionProvider,
            IMapper mapper,
            IDomainService<BasePersonalAccountDto> basePersonalAccountDtoDomain)
        {
            this.container = container;
            this.sessionProvider = sessionProvider;
            this.mapper = mapper;
            this.basePersonalAccountDtoDomain = basePersonalAccountDtoDomain;

            // по-умолчанию работаем с сессией без состояния
            this.useStatelessSession = true;
        }

        /// <inheritdoc />
        public void InitCache()
        {
            this.accounts = new HashSet<BasePersonalAccount>();
            this.maxIdExistsPersonalAccount = this.basePersonalAccountDtoDomain.GetAll().Max(x => x.Id);
        }

        /// <inheritdoc />
        public void Clear()
        {
            this.InitCache();
        }

        /// <inheritdoc />
        public void AddPersonalAccount(BasePersonalAccount account)
        {
            if (this.maxIdExistsPersonalAccount == 0)
            {
                this.InitCache();
            }

            this.accounts.Add(account);
        }

        /// <inheritdoc />
        public void ApplyChanges(bool rebuildCache)
        {
            var listToSave = new List<PersistentObject>();
            var listToCreate = new List<BasePersonalAccountDto>();

            var persAccToUpdate = this.accounts.Where(x => x.Id <= this.maxIdExistsPersonalAccount).ToDictionary(x => x.Id);
            var persAccToUpdateIds = persAccToUpdate.Keys.ToArray();

            foreach (var accountDto in this.basePersonalAccountDtoDomain.GetAll().WhereContains(x => x.Id, persAccToUpdateIds))
            {
                listToSave.Add(accountDto.UpdateMe(persAccToUpdate.Get(accountDto.Id), this.mapper));
            }

            listToCreate.AddRange(this.accounts
                .Where(x => !persAccToUpdateIds.Contains(x.Id))
                .Select(x => BasePersonalAccountDto.FromAccount(x, this.mapper)));

            this.ProcessChangesInternal(listToSave, listToCreate);

            if (rebuildCache)
            {
                // заново инициализируем кэш
                this.InitCache();
            }
            else
            {
                // или вручную добавляем
                this.maxIdExistsPersonalAccount = listToSave.SafeMax(x => x.Id);
            }
        }

        private void ProcessChangesInternal(List<PersistentObject> listToSave, List<BasePersonalAccountDto> listToCreate)
        {
            if (this.useStatelessSession)
            {
                this.container.InStatelessTransaction(stateless =>
                {

                    listToSave.ForEach(stateless.Update);
                    listToCreate.ForEach(x => this.CreateQuery(stateless.CreateSQLQuery, x).ExecuteUpdate());
                });
            }
            else
            {
                this.container.InTransaction(() =>
                {
                    listToSave.ForEach(this.basePersonalAccountDtoDomain.Update);
                    var session = this.container.Resolve<ISessionProvider>().GetCurrentSession();
                    listToCreate.ForEach(x => this.CreateQuery(session.CreateSQLQuery, x).ExecuteUpdate());
                });
            }

        }

        /// <inheritdoc />
        public IDataResult MassCreatePersonalAccountDto(bool force)
        {
            const string existsRelQuery = @"SELECT count(*) > 0
                FROM information_schema.tables 
                WHERE table_name = 'regop_pers_acc_dto';";
            const string errorMessage = "Актуализация реестра лицевых уже производится в данный момент";

            if (!force)
            {
                var relationExists = this.sessionProvider.GetCurrentSession().CreateSQLQuery(existsRelQuery).UniqueResult().ToBool();

                if (relationExists)
                {
                    return new BaseDataResult();
                }
            }

            if (!Monitor.TryEnter(MassPersonalAccountDtoService.SyncRoot))
            {
                return BaseDataResult.Error(errorMessage);
            }

            try
            {

                this.sessionProvider.InStatelessConnectionTransaction(
                    (connection, transaction) =>
                    {
                        foreach (var createDtoQuery in this.GetCreateDtoQueries())
                        {
                            connection.Execute(createDtoQuery, transaction: transaction);
                        }

                        var countPersAcc = connection.ExecuteScalar<long>("select count(*) from regop_pers_acc");
                        var countDto = connection.ExecuteScalar<long>("select count(*) from regop_pers_acc_dto");

                        if (countDto != countPersAcc)
                        {
                            throw new ValidationException("Выявлена ошибка не соответствия количества лицевых счетов в кэше. Попробуйте ещё раз");
                        }
                    });

                return new BaseDataResult();
            }
            finally
            {
                Monitor.Exit(MassPersonalAccountDtoService.SyncRoot);
            }
        }

        /// <summary>
        /// Использовать сессию без состояния
        /// </summary>
        public void UseStatelessSession(bool useStateless = true)
        {
            this.useStatelessSession = useStateless;
        }

        private IEnumerable<string> GetCreateDtoQueries()
        {
            var listResult = new List<string>
            {
                "DROP TABLE IF EXISTS regop_pers_acc_dto;",

                @"CREATE TABLE regop_pers_acc_dto as
                SELECT
                  pa.Id                                   AS Id,
                  pa.ACC_NUM                              AS ACC_NUM,
                  pa.UNIFIED_ACC_NUM                              AS GIS_ACC_NUM,
                  pa.REGOP_PERS_ACC_EXTSYST               AS REGOP_PERS_ACC_EXTSYST,
                  pa.OPEN_DATE                            AS OPEN_DATE,
                  case pa.CLOSE_DATE when '-infinity' 
                       then null else pa.CLOSE_DATE end   AS CLOSE_DATE,
                  pa.AREA_SHARE                           AS AREA_SHARE,
                  pa.ACC_OWNER_ID                         AS ACC_OWNER_ID,
                  pa.ROOM_ID                              AS ROOM_ID,
                  pa.DIGITAL_RECEIPT                      AS DIGITAL_RECEIPT,
                  pa.IS_NOT_DEBTOR                        AS IS_NOT_DEBTOR,
                  case coalesce(indivOwner.EMAIL, contragent.EMAIL, '')
                      when '' 
                      then false
                      else true end                       AS HAVE_EMAIL,

                  owner.OWNER_TYPE                        AS OWNER_TYPE,
                  coalesce(owner.NAME, contragent.NAME)   AS NAME,
                  owner.privileged_category               AS PRIVILEGED_CATEGORY_ID,
                  state.start_state and owner.active_accounts_count = 1
                                                          AS HAS_ONLY_ROOM_WITH_OPEN_STATE,

                  room.RO_ID                              AS RO_ID,
                  room.CAREA                              AS CAREA,
                  room.CROOM_NUM                          AS CROOM_NUM,
                  room.CHAMBER_NUM                          AS CHAMBER_NUM,

                  ro.address || ', кв. ' || room.croom_num ||
                      COALESCE((', ком. ' || NULLIF(room.chamber_num, ''))::VARCHAR, ''::VARCHAR)
                                                          AS ROOM_ADRESS,

                  ro.ADDRESS                              AS ADDRESS,
                  ro.AREA_MKD                             AS AREA_MKD,
                  coalesce(ro.ACC_FORM_VARIANT, -1)       AS ACC_FORM_VARIANT,


                  municipali6_.ID                         AS MO_ID,
                  municipali6_.NAME                       AS MO_NAME,
                  municipali7_.ID                         AS STL_ID,
                  municipali7_.NAME                       AS STL_NAME,
                  coalesce(pa.STATE_ID,0)                 AS STATE_ID
                 
                FROM REGOP_PERS_ACC pa INNER JOIN B4_STATE state ON pa.STATE_ID = state.ID
                  LEFT OUTER JOIN REGOP_PERS_ACC_OWNER owner ON pa.ACC_OWNER_ID = owner.Id
                  LEFT OUTER JOIN REGOP_INDIVIDUAL_ACC_OWN indivOwner ON owner.Id = indivOwner.Id
                  LEFT OUTER JOIN REGOP_LEGAL_ACC_OWN legalOwner ON owner.Id = legalOwner.Id
                  LEFT OUTER JOIN GKH_CONTRAGENT contragent ON legalOwner.CONTRAGENT_ID = contragent.Id
                  LEFT OUTER JOIN GKH_ROOM room ON pa.ROOM_ID = room.Id
                  LEFT OUTER JOIN GKH_REALITY_OBJECT ro ON room.RO_ID = ro.Id
                  LEFT OUTER JOIN GKH_DICT_MUNICIPALITY municipali6_ ON ro.MUNICIPALITY_ID = municipali6_.Id
                  LEFT OUTER JOIN GKH_DICT_MUNICIPALITY municipali7_ ON ro.STL_MUNICIPALITY_ID = municipali7_.Id; ",

                "CREATE UNIQUE INDEX ON regop_pers_acc_dto (Id);",
                "CREATE UNIQUE INDEX ON regop_pers_acc_dto (ACC_NUM);",
                "CREATE INDEX ON regop_pers_acc_dto (RO_ID);",
                "CREATE INDEX ON regop_pers_acc_dto (MO_ID);",
                "CREATE INDEX ON regop_pers_acc_dto (MO_NAME);",
                "CREATE INDEX ON regop_pers_acc_dto (STL_ID);",
                "CREATE INDEX ON regop_pers_acc_dto (STL_NAME);",
                "CREATE INDEX ON regop_pers_acc_dto (ACC_OWNER_ID);",
                "CREATE INDEX ON regop_pers_acc_dto (OWNER_TYPE);",
                "CREATE INDEX ON regop_pers_acc_dto (NAME);",
                "CREATE INDEX ON regop_pers_acc_dto (ROOM_ADRESS);",
                "ALTER TABLE regop_pers_acc_dto ADD CONSTRAINT fk_regop_pers_acc_dto FOREIGN KEY (id) REFERENCES regop_pers_acc (id) ON DELETE CASCADE;",
                "ANALYZE regop_pers_acc_dto;"
            };

            return listResult;
        }

        private IQuery CreateQuery(Func<string, IQuery> queryGetter, BasePersonalAccountDto account)
        {
            const string sqlQueryForInsert =
                "INSERT INTO regop_pers_acc_dto" +
                " (ID, ACC_NUM, REGOP_PERS_ACC_EXTSYST, OPEN_DATE, CLOSE_DATE, AREA_SHARE, ACC_OWNER_ID, ROOM_ID, GIS_ACC_NUM, DIGITAL_RECEIPT," +
                "HAVE_EMAIL, IS_NOT_DEBTOR, OWNER_TYPE, NAME, PRIVILEGED_CATEGORY_ID, HAS_ONLY_ROOM_WITH_OPEN_STATE, RO_ID, CAREA, CROOM_NUM, CHAMBER_NUM," +
                "ROOM_ADRESS, ADDRESS, AREA_MKD, ACC_FORM_VARIANT, MO_ID, MO_NAME, STL_ID, STL_NAME, STATE_ID)" +
                " VALUES " +
                "(:ID, :ACC_NUM, :REGOP_PERS_ACC_EXTSYST, :OPEN_DATE, :CLOSE_DATE, :AREA_SHARE, :ACC_OWNER_ID, :ROOM_ID, :GIS_ACC_NUM, :DIGITAL_RECEIPT," +
                ":HAVE_EMAIL, :IS_NOT_DEBTOR, :OWNER_TYPE, :NAME, :PRIVILEGED_CATEGORY_ID, :HAS_ONLY_ROOM_WITH_OPEN_STATE, :RO_ID, :CAREA, :CROOM_NUM, :CHAMBER_NUM," +
                ":ROOM_ADRESS, :ADDRESS, :AREA_MKD, :ACC_FORM_VARIANT, :MO_ID, :MO_NAME, :STL_ID, :STL_NAME, :STATE_ID)";

            return queryGetter(sqlQueryForInsert)
                .SetInt64("ID", account.Id)
                .SetInt64("ACC_OWNER_ID", account.OwnerId)
                .SetInt64("ROOM_ID", account.RoomId)
                .SetInt64("RO_ID", account.RoId)
                .SetInt64("MO_ID", account.MuId)
                .SetInt64("STATE_ID", account.State.Id)
                .SetInt32("OWNER_TYPE", (int)account.OwnerType)
                .SetInt32("DIGITAL_RECEIPT", (int)account.DigitalReceipt)
                .SetBoolean("HAVE_EMAIL", account.HaveEmail)
                .SetBoolean("IS_NOT_DEBTOR", account.IsNotDebtor)
                .SetInt32("ACC_FORM_VARIANT", (int)account.AccountFormationVariant)
                .SetString("ACC_NUM", account.PersonalAccountNum)
                .SetString("GIS_ACC_NUM", account.UnifiedAccountNumber)
                .SetString("REGOP_PERS_ACC_EXTSYST", account.PersAccNumExternalSystems)
                .SetString("NAME", account.AccountOwner)
                .SetString("CROOM_NUM", account.RoomNum)
                .SetString("CHAMBER_NUM", account.ChamberNum)
                .SetString("ROOM_ADRESS", account.RoomAddress)
                .SetString("ADDRESS", account.Address)
                .SetString("MO_NAME", account.Municipality)
                .SetString("STL_NAME", account.Settlement)
                .SetDateTime("OPEN_DATE", account.OpenDate)
                .SetParameter("CLOSE_DATE", account.CloseDate)
                .SetParameter("CAREA", account.Area)
                .SetParameter("AREA_MKD", account.AreaMkd)
                .SetParameter("STL_ID", account.SettleId)
                .SetParameter("PRIVILEGED_CATEGORY_ID", account.PrivilegedCategoryId)
                .SetDecimal("AREA_SHARE", account.AreaShare)
                .SetBoolean("HAS_ONLY_ROOM_WITH_OPEN_STATE", account.HasOnlyOneRoomWithOpenState);
        }
    }
}