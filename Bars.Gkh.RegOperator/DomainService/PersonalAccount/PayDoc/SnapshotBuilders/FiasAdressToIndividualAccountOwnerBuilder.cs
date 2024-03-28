namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.SnapshotBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.DataProviders;
    using Bars.Gkh.RegOperator.DataProviders.Meta;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.SnapshotCreators;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Tasks.PaymentDocuments;

    using NHibernate;
    using NHibernate.Linq;
    using Entities;

    /// <summary>
    /// Источник по фактическому адресу абонента
    /// </summary>
    class FiasAdressToIndividualAccountOwnerBuilder : AbstractSnapshotBuilder
    {
        /// <summary>
        /// Id
        /// </summary>
        public static string Id => nameof(FiasAdressToIndividualAccountOwnerBuilder);

        /// <summary>
        /// Код источника
        /// </summary>
        public override string Code => FiasAdressToIndividualAccountOwnerBuilder.Id;

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name => "Источник по фактическому адресу абонента";

        /// <summary>
        /// Описание заполняемых полей 
        /// </summary>
        public override string Description => "АгентДоставФактАдрес, НаселенныйПунктФактАдрес, УлицаФактАдрес, " +
                                        "ДомФактАдрес, ЛитерФактАдрес, КорпусФактАдрес, СекцияФактАдрес, НомерПомещенияФактАдрес";

        private Dictionary<long, string> FiasAdressToIndividualAccountOwnerBuilserDict;

        private Dictionary<long, FiasFactproxy> FiasAdressToIndividualAccountOwnerDict;

        /// <summary>
        /// Инициализация кэша
        /// </summary>
        /// <param name="docCache">Кэш, в котором регистрируются сущности</param>
        /// <param name="mainInfo">Основной запрос</param>
        /// <param name="session">Сессия</param>
        public override void InitCache(DocCache docCache, PersonalAccountRecord[] mainInfo, IStatelessSession session)
        {
            var accIds = mainInfo.Select(x => x.AccountId)
                .Distinct()
                .ToArray();

            var accOwnerIDs = session.Query<BasePersonalAccount>()
                .Where(x => accIds.Contains(x.Id))
                .ToDictionary(x => x.Id, x => x.AccountOwner.Id);

            var indAccOwnerQuery = session.Query<IndividualAccountOwner>()
                .Where(x => accOwnerIDs.Values.Contains(x.Id));

            var roIDs = accOwnerIDs
                .ToDictionary(
                    x => x.Key,
                    x => indAccOwnerQuery.Where(z => z.Id == x.Value)
                        .Select(y => (long?) y.RealityObject.Id).FirstOrDefault() ?? 0);

            var delAgents = session.Query<DeliveryAgentRealObj>()
                .Fetch(x => x.DeliveryAgent)
                .ThenFetch(x => x.Contragent)
                .Where(x => roIDs.Values.Contains(x.RealityObject.Id))
                .ToDictionary(x => x.RealityObject.Id, x => x.DeliveryAgent.Contragent.Name);

            docCache.Cache.RegisterDto<DeliveryAgentRealObj, PersonalAccountPerRealityObject>()
                .SetQueryBuilder(r =>
                {
                    return roIDs
                        .Select(
                            x =>
                                new PersonalAccountPerRealityObject
                                {
                                    Id = x.Key,
                                    Name = delAgents.Get(x.Value)
                                })
                        .AsQueryable();
                });

            var accOwneIDs = session.Query<BasePersonalAccount>()
               .Where(x => accIds.Contains(x.Id))
               .Select(x => new
               {
                   x.Id,
                   (x.AccountOwner as IndividualAccountOwner).FiasFactAddress
               })
               .ToDictionary(x => x.Id, x => x.FiasFactAddress);

            docCache.Cache.RegisterDto<IndividualAccountOwner, FiasFactproxy>()
            .SetQueryBuilder(r =>
            {
                return accOwneIDs
                .Where(x => x.Value != null)
                    .Select(
                        x =>
                            new FiasFactproxy
                            {
                                Id = x.Key,
                                PlaceName = x.Value.PlaceName,
                                StreetName = x.Value.StreetName,
                                House = x.Value.House,
                                Letter = x.Value.Letter,
                                Housing = x.Value.Housing,
                                Building = x.Value.Building,
                                Flat = x.Value.Flat
                            })
                    .AsQueryable();
            });
        }

        /// <summary>
        /// Получение конкретных данных из кэша для последующей работы
        /// </summary>
        /// <param name="creator">Инициатор</param>
        public override void WarmCache(SnapshotCreator creator)
        {
            this.FiasAdressToIndividualAccountOwnerBuilserDict = creator.Cache.GetCache<PersonalAccountPerRealityObject>().GetEntities()
                .ToDictionary(x => x.Id, x => x.Name);

            this.FiasAdressToIndividualAccountOwnerDict = creator.Cache.GetCache<FiasFactproxy>().GetEntities()
                .ToDictionary(x => x.Id, x => x);
        }

        /// <summary>
        /// Заполнение одной записи модели с использованием данных, полученных в WarmCache 
        /// </summary>
        /// <param name="creator">Инициатор</param>
        /// <param name="record">Запись</param>
        /// <param name="account">Информация о лс</param>
        public override void FillRecord(
            SnapshotCreator creator,
            InvoiceInfo record,
            PersonalAccountPaymentDocProxy account)
        {
            record.АгентДоставФактАдрес = this.FiasAdressToIndividualAccountOwnerBuilserDict.Get(account.Id);

            var address = this.FiasAdressToIndividualAccountOwnerDict.Get(account.Id);
            if (address != null)
            {
                record.НаселенныйПунктФактАдрес = address.PlaceName;
                record.УлицаФактАдрес = address.StreetName;
                record.ДомФактАдрес = address.House;
                record.ЛитерФактАдрес = address.Letter;
                record.КорпусФактАдрес = address.Housing;
                record.СекцияФактАдрес = address.Building;
                record.НомерПомещенияФактАдрес = address.Flat;
            }
            
        }
    }

    internal class PersonalAccountPerRealityObject
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    internal class FiasFactproxy
    {
        public long Id { get; set; }
        public string PlaceName { get; set; }
        public string StreetName { get; set; }
        public string House { get; set; }
        public string Letter { get; set; }
        public string Housing { get; set; }
        public string Building { get; set; }
        public string Flat { get; set; }
    }
}