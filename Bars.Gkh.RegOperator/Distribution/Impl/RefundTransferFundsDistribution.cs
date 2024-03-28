namespace Bars.Gkh.RegOperator.Distribution.Impl
{
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.Domain.Extensions;
    using Bars.Gkh.RegOperator.Domain.Repository.RealityObjectAccount;
    using Bars.Gkh.RegOperator.Domain.Repository.Transfers;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Entities.Wallet;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Repositories.ChargePeriod;

    /// <summary>
    /// Возврат перечисленных средств
    /// </summary>
    public class RefundTransferFundsDistribution : AbstractRefundTransferCtrDistribution
    {
        public RefundTransferFundsDistribution(
            IDomainService<TransferCtr> transferCtrDomain,
            IRealityObjectPaymentAccountRepository accountRepo,
            IChargePeriodRepository periodRepo,
            IDomainService<Wallet> walletDomain,
            IDomainService<MoneyOperation> moneyOperationDomain,
            IDomainService<Transfer> transferDomain,
            ITransferRepository<RealityObjectTransfer> transferRepo,
            IDomainService<DistributionDetail> detailDomain,
            IDomainService<TransferCtrPaymentDetail> transferCtrDetailDomain)
            : base(transferCtrDomain,
                accountRepo,
                periodRepo,
                walletDomain,
                moneyOperationDomain,
                transferDomain,
                transferRepo,
                detailDomain,
                transferCtrDetailDomain)
        {
        }

        /// <summary>
        /// Код распределения
        /// </summary>
        public override DistributionCode DistributionCode => DistributionCode.RefundTransferFundsDistribution;

        /// <summary>
        /// Идентификатор права доступа
        /// </summary>
        public override string PermissionId
        {
            get { return "GkhRegOp.Accounts.SuspenseAccount.EnrollmentTypes.RefundTransferFundsDistribution"; }
        }

        /// <summary>
        /// MoneyOperation Reason
        /// </summary>
        public override string MoneyOperationReason => "Возврат перечисленных средств";

        public override string GetApplyTransferReason(TransferCtrPaymentDetail transferCtrDetail, RealityObjectPaymentAccount account)
        {
            return "Возврат перечисленных средств ({0})".FormatUsing(transferCtrDetail.Wallet.GetWalletName(this.Container, account));
        }

        public override string GetUndoTransferReason(TransferCtrPaymentDetail transferCtrDetail, RealityObjectPaymentAccount account)
        {
            return "Отмена возврата перечисленных средств ({0})".FormatUsing(transferCtrDetail.Wallet.GetWalletName(this.Container, account));
        }
    }
}