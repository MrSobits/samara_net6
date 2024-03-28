namespace Bars.Gkh.RegOperator.Domain.Repository.RealityObjectAccount
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Analytics.Extensions;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Domain.AggregationRoots;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;

    using NHibernate;
    using NHibernate.Linq;

    /// <summary>
    /// Репозиториq для корней агрегации счетов дома
    /// </summary>
    public class RealtyObjectPaymentRootRepository : IRealtyObjectPaymentRootRepository
    {
        private readonly IDomainService<RealityObjectPaymentAccount> paymentAccRepo;
        private readonly IDomainService<RealityObjectChargeAccount> chargeAccRepo;
        private readonly IDomainService<Transfer> transferRepo;
        private readonly ISessionProvider sessions;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="transferRepo">Домен-сервис <see cref="Transfer"/></param>
        /// <param name="paymentAccRepo">Домен-сервис <see cref="RealityObjectPaymentAccount"/></param>
        /// <param name="chargeAccRepo">Домен-сервис <see cref="RealityObjectChargeAccount"/></param>
        /// <param name="sessions">Провайдер сессий</param>
        public RealtyObjectPaymentRootRepository(
            IDomainService<Transfer> transferRepo,
            IDomainService<RealityObjectPaymentAccount> paymentAccRepo,
            IDomainService<RealityObjectChargeAccount> chargeAccRepo,
            ISessionProvider sessions)
        {
            this.transferRepo = transferRepo;
            this.paymentAccRepo = paymentAccRepo;
            this.chargeAccRepo = chargeAccRepo;
            this.sessions = sessions;
        }

        /// <inheritdoc />
        public IEnumerable<RealtyObjectPaymentRoot> GetByRealityObjects(IEnumerable<RealityObject> realityObjects)
        {
            var ids = realityObjects.Select(x => x.Id).Distinct().ToList();

            var result = new List<RealtyObjectPaymentRoot>();

            var oldFlush = this.sessions.GetCurrentSession().FlushMode;
            this.sessions.GetCurrentSession().FlushMode = FlushMode.Never;

            foreach (var idSection in ids.Section(555))
            {
                var paymentAccs = this.paymentAccRepo.GetAll()
                    .Where(x => idSection.Contains(x.RealityObject.Id))
                    .FetchAllWallets().ToList();

                var chargeAccounts = this.chargeAccRepo.GetAll()
                    .Where(x => idSection.Contains(x.RealityObject.Id))
                    .FetchMany(x => x.Operations).ToList();

                result.AddRange(
                    paymentAccs.LeftJoin(
                        chargeAccounts,
                        pa => pa.RealityObject.Id,
                        ca => ca.RealityObject.Id,
                        (pa, ca) => new RealtyObjectPaymentRoot(pa, ca)));
            }

            this.sessions.GetCurrentSession().FlushMode = oldFlush;

            return result;
        }

        /// <inheritdoc />
        public void Update(RealtyObjectPaymentRoot root)
        {
            this.paymentAccRepo.Update(root.PaymentAccount);
            this.chargeAccRepo.Update(root.ChargeAccount);

            root.Transfers.ForEach(this.transferRepo.Save);
            root.ClearTransfers();
        }
    }
}