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
    using Bars.GkhCr.Entities;

    /// <summary>
    /// Распределение субсидии фонда
    /// </summary>
    public class FundSubsidyDistribution : AbstractRealtyAccountDistribution
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public FundSubsidyDistribution(
            IDomainService<RealityObjectPaymentAccount> ropayAccDomain,
            IDomainService<RealityObjectTransfer> transferDomain,
            ITransferRepository<RealityObjectTransfer> transferRepo,
            IDomainService<DistributionDetail> detailDomain,
            IDomainService<MoneyOperation> moneyOperationDomain,
            IDomainService<CalcAccountRealityObject> calcAccountRoDomain,
            IDomainService<ObjectCr> objectCrDomain,
            IDomainService<Room> roomDomain)
            : base(ropayAccDomain, transferDomain, transferRepo, detailDomain, moneyOperationDomain, calcAccountRoDomain, objectCrDomain, roomDomain)
        {
        }

        /// <summary>
        /// Код распределения
        /// </summary>
        public override DistributionCode DistributionCode => DistributionCode.FundSubsidyDistribution;

        /// <summary>
        /// Идентификатор права доступа
        /// </summary>
        public override string PermissionId => "GkhRegOp.Accounts.SuspenseAccount.EnrollmentTypes.FundSubsidy";

        /// <summary>
        /// Селектор кошелька
        /// </summary>
        protected override Wallet GetWallet(RealityObjectPaymentAccount realityObjectPaymentAccount)
        {
            return realityObjectPaymentAccount.FundSubsidyWallet;
        }

        /// <summary>
        /// Получить счета оплат жилых домов
        /// </summary>
        /// <param name="query">трансферы, связанные со счетом нвс</param>
        /// <returns></returns>
        protected override IEnumerable<RealityObjectPaymentAccount> GetPaymentAccounts(IQueryable<Transfer> query)
        {
            return this.RopayAccDomain.GetAll()
                .Where(y => query.Any(x => x.Owner.Id == y.Id))
                .ToArray();
        }
    }
}