﻿namespace Bars.Gkh.RegOperator.Distribution.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.RegOperator.Domain.Repository.Transfers;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Entities.Wallet;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Repositories.ChargePeriod;
    using Bars.GkhCr.Entities;

    public class RefundTargetSubsidyDistribution : AbstractRealtyAccountRefundDistribution
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="ropayAccDomain">DomainService <see cref="RefundTargetSubsidyDistribution" /></param>
        /// <param name="transferDomain"></param>
        /// <param name="transferRepo"></param>
        /// <param name="periodRepo"></param>
        /// <param name="detailDomain"></param>
        /// <param name="moneyOperationDomain"></param>
        public RefundTargetSubsidyDistribution(
            IDomainService<RealityObjectPaymentAccount> ropayAccDomain,
            IDomainService<RealityObjectTransfer> transferDomain,
            ITransferRepository<RealityObjectTransfer> transferRepo,
            IChargePeriodRepository periodRepo,
            IDomainService<DistributionDetail> detailDomain,
            IDomainService<MoneyOperation> moneyOperationDomain,
            IDomainService<BasePersonalAccount> personalAccountDomain,
            IDomainService<CalcAccountRealityObject> calcAccountRoDomain,
            IDomainService<ObjectCr> objectCrDomain)
            : base(ropayAccDomain,
                transferDomain,
                transferRepo,
                periodRepo,
                detailDomain,
                moneyOperationDomain,
                personalAccountDomain,
                calcAccountRoDomain,
                objectCrDomain)
        {
        }

        /// <summary>
        /// Код распределения
        /// </summary>
        public override DistributionCode DistributionCode => DistributionCode.RefundTargetSubsidyDistribution;

        /// <summary>
        /// Идентификатор права доступа
        /// </summary>
        public override string PermissionId
        {
            get { return "GkhRegOp.Accounts.SuspenseAccount.EnrollmentTypes.RefundTargetSubsidy"; }
        }

        /// <summary>
        /// Селектор кошелька
        /// </summary>
        protected override Wallet GetWallet(RealityObjectPaymentAccount realityObjectPaymentAccount)
        {
            return realityObjectPaymentAccount.TargetSubsidyWallet;
        }

        /// <summary>
        /// Получить счета оплат жилых домов
        /// </summary>
        /// <param name="query">трансферы, связанные со счетом нвс</param>
        /// <returns></returns>
        protected override IEnumerable<RealityObjectPaymentAccount> GetPaymentAccounts(IQueryable<Transfer> query)
        {
            return this.RopayAccDomain.GetAll()
                .Where(y => query.Any(x => x.SourceGuid == y.TargetSubsidyWallet.WalletGuid))
                .ToArray();
        }
    }
}