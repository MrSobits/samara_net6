namespace Bars.Gkh.RegOperator.Distribution.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Domain.Repository.Transfers;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Entities.Wallet;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Repositories.ChargePeriod;
    using Bars.GkhCr.Entities;

    /// <summary>
    /// Распределение стимулирующей субсидии
    /// </summary>
    public class StimulateSubsidyDistribution : AbstractRealtyAccountDistribution
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="ropayAccDomain">DomainService <see cref="RealityObjectPaymentAccount" /></param>
        /// <param name="transferDomain"></param>
        /// <param name="transferRepo"></param>
        /// <param name="periodRepo"></param>
        /// <param name="detailDomain"></param>
        public StimulateSubsidyDistribution(
            IDomainService<RealityObjectPaymentAccount> ropayAccDomain,
            IDomainService<RealityObjectTransfer> transferDomain,
            ITransferRepository<RealityObjectTransfer> transferRepo,
            IChargePeriodRepository periodRepo,
            IDomainService<DistributionDetail> detailDomain,
            IDomainService<MoneyOperation> moneyOperationDomain,
            IDomainService<BasePersonalAccount> personalAccountDomain,
            IDomainService<CalcAccountRealityObject> calcAccountRoDomain,
            IDomainService<ObjectCr> objectCrDomain,
            IDomainService<Room> roomDomain)
            : base(ropayAccDomain, transferDomain, transferRepo, detailDomain, moneyOperationDomain, calcAccountRoDomain, objectCrDomain, roomDomain)
        {
        }

        /// <summary>
        /// Код распределения
        /// </summary>
        public override DistributionCode DistributionCode => DistributionCode.StimulateSubsidyDistribution;

        /// <summary>
        /// Идентификатор права доступа
        /// </summary>
        public override string PermissionId => "GkhRegOp.Accounts.SuspenseAccount.EnrollmentTypes.StimulatingSubsidy";

        /// <summary>
        /// Селектор кошелька
        /// </summary>
        protected override Wallet GetWallet(RealityObjectPaymentAccount realityObjectPaymentAccount)
        {
            return realityObjectPaymentAccount.StimulateSubsidyWallet;
        }

        protected override IEnumerable<RealityObjectPaymentAccount> GetPaymentAccounts(IQueryable<Transfer> query)
        {
            return this.RopayAccDomain.GetAll()
                .Where(y => query.Any(x => x.Owner.Id == y.Id))
                .ToArray();
        }
    }
}