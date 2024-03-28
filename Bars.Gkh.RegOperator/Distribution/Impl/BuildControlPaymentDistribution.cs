namespace Bars.Gkh.RegOperator.Distribution.Impl
{
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.Domain.Repository.RealityObjectAccount;
    using Bars.Gkh.RegOperator.Domain.Repository.Transfers;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Refactor;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Entities.Wallet;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Repositories.ChargePeriod;

    /// <summary>
    /// Оплата Стройконтроль
    /// </summary>
    public class BuildControlPaymentDistribution : AbstractTransferCtrDistribution
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public BuildControlPaymentDistribution(
            IDomainService<TransferCtr> transferCtrDomain,
            IDomainService<MoneyLock> moneyLockDomain,
            IRealityObjectPaymentAccountRepository accountRepo,
            IChargePeriodRepository periodRepo,
            IDomainService<Wallet> walletDomain,
            IDomainService<MoneyOperation> moneyOperationDomain,
            ITransferRepository<RealityObjectTransfer> transferRepo,
            IDomainService<DistributionDetail> detailDomain)
            : base(transferCtrDomain,
                moneyLockDomain,
                accountRepo,
                periodRepo,
                walletDomain,
                moneyOperationDomain,
                transferRepo,
                detailDomain)
        {
        }

        /// <summary>
        /// Код распределения
        /// </summary>
        public override DistributionCode DistributionCode => DistributionCode.BuildControlPaymentDistribution;

        /// <summary>
        /// Идентификатор права доступа
        /// </summary>
        public override string PermissionId => "GkhRegOp.Accounts.SuspenseAccount.EnrollmentTypes.BuildControlPayment";

        /// <summary>
        /// MoneyOperation Reason
        /// </summary>
        public override string MoneyOperationReason => "Оплата Стройконтроль";

        public override string GetApplyTransferReason(TransferCtr transferCtr, MoneyLock mLock)
        {
            return "Оплата Стройконтроль ({0})".FormatUsing(mLock.SourceName);
        }

        public override string GetUndoTransferReason(TransferCtr transferCtr, MoneyLock mLock)
        {
            return "Отмена оплаты Стройконтроль ({0})".FormatUsing(mLock.SourceName);
        }
    }
}