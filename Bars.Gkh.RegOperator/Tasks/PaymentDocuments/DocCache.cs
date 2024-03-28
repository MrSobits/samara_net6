namespace Bars.Gkh.RegOperator.Tasks.PaymentDocuments
{
    using System;

    using B4.DataAccess;
    using B4.Utils;
    using Bars.Gkh.Domain.Cache;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;
    using Bars.Gkh.RegOperator.Entities;
    using Castle.Windsor;
    using NHibernate.Linq;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.Gkh.RegOperator.Domain.ParametersVersioning;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.SnapshotBuilders;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;

    /// <summary>
    /// Кэш данных
    /// </summary>
    public class DocCache : IDisposable
    {
        private readonly IWindsorContainer container;

        /// <summary>
        /// Период
        /// </summary>
        internal readonly IPeriod Period;

        /// <summary>
        /// Id лс
        /// </summary>
        internal readonly long[] AccountIds;

        /// <summary>
        /// Словарь лс+владелец
        /// </summary>
        internal readonly Dictionary<long, long> AccountOwnershipDict;

        /// <summary>
        /// Кэш
        /// </summary>
        internal readonly GkhCache Cache;

        /// <summary>
        /// Кэш для получения актуальных тарифа и площади 
        /// </summary>
        internal readonly ITariffAreaCache TariffAreaCache;

        /// <summary>
        /// Провайдер сессии
        /// </summary>
        internal readonly ISessionProvider SessionProvider;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        /// <param name="period">Период</param>
        /// <param name="primarySources">Первоисточник квитанции: список счетов и их владельцев</param>
        /// <param name="builders">Источники данных</param>
        public DocCache(
            IWindsorContainer container, 
            IPeriod period, 
            IEnumerable<PayDocPrimarySource> primarySources,
            List<ISnapshotBuilder> builders)
        {
            this.container = container;
            this.Period = period;

            this.AccountOwnershipDict = primarySources
                .GroupBy(x => x.AccountId)
                .ToDictionary(x => x.Key, x => x.Select(y => y.OwnerId).First());

            this.AccountIds = primarySources.Select(x => x.AccountId).ToArray();

            this.Cache = container.Resolve<GkhCache>();
            this.TariffAreaCache = container.Resolve<ITariffAreaCache>();
            this.SessionProvider = container.Resolve<ISessionProvider>();

            this.Init(builders);
        }

        /// <summary>
        /// Получить кэш
        /// </summary>
        /// <returns>Кэш</returns>
        public GkhCache GetCache()
        {
            return this.Cache;
        }
        
        /// <summary>
        /// Освобождение ресурсов
        /// </summary>
        public void Dispose()
        {
            if (this.Cache != null)
            {
                this.Cache.Dispose();
            }

            if (this.TariffAreaCache != null)
            {
                this.TariffAreaCache.Dispose();
            }
            
            if (this.SessionProvider != null)
            {
                this.SessionProvider.CloseCurrentSession();
            }

            this.container.Release(this.Cache);
            this.container.Release(this.TariffAreaCache);
            this.container.Release(this.SessionProvider);
        }

        private void Init(List<ISnapshotBuilder> builders)
        {
            if (this.AccountIds.IsEmpty())
            {
                return;
            }

            this.Cache.IsActive = true;

            using (var session = this.SessionProvider.OpenStatelessSession())
            {
                this.Cache.RegisterEntity<RegOperator>()
                    .SetQueryBuilder(r => r.GetAll().Fetch(x => x.Contragent))
                    .MakeDictionary("ReceiptRegopByState", x => x.Contragent.ContragentState.ToString());

                //лс
                this.Cache.RegisterEntity<BasePersonalAccount>()
                    .SetQueryBuilder(
                        r => r.GetAll()
                            .WhereContains(x => x.Id, this.AccountIds)
                            .Fetch(x => x.Room)
                            .ThenFetch(t => t.RealityObject)
                            .ThenFetch(t => t.FiasAddress)
                            .Fetch(x => x.Room)
                            .ThenFetch(t => t.RealityObject)
                            .ThenFetch(t => t.Municipality)
                            .Fetch(x => x.Room)
                            .ThenFetch(t => t.RealityObject)
                            .ThenFetch(t => t.MoSettlement)
                            .Fetch(x => x.SocialSupportWallet)
                            .Fetch(x => x.State));

                this.Cache.RegisterEntity<AccountPaymentInfoSnapshot>()
                    .SetQueryBuilder(
                        r => r.GetAll()
                            .Fetch(x => x.Snapshot)
                            .Where(x => x.Snapshot.IsBase)
                            .Where(x => x.Snapshot.Period.Id == this.Period.Id)
                            .WhereContains(x => x.AccountId, this.AccountIds));

                this.Cache.RegisterEntity<AccountOwnershipHistory>()
                    .SetQueryBuilder(
                        r => r.GetAll()
                            .Where(x => x.Date <= (this.Period.EndDate ?? DateTime.Today))
                            .WhereContains(x => x.PersonalAccount.Id, this.AccountIds));

                var primeQuery = session.Query<BasePersonalAccount>().WhereContains(x => x.Id, this.AccountIds);
                var primeQueryPreLoaded = primeQuery.Select(x => new PersonalAccountRecord
                    {
                        RoomId = x.Room.Id,
                        RealityObjectId = x.Room.RealityObject.Id,
                        AccountId = x.Id
                    })
                    .ToArray();

                foreach (var builder in builders)
                {
                    builder.InitCache(this, primeQueryPreLoaded, session);
                }

                this.Cache.UpdateStateless(this.SessionProvider);  
            }
        } 
    }
}