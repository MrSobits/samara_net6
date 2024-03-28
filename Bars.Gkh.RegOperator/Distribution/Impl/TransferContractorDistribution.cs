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
    /// Распределение оплат подрядчику
    /// </summary>
    public class TransferContractorDistribution : AbstractTransferCtrDistribution
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public TransferContractorDistribution(
            IDomainService<TransferCtr> transferCtrDomain,
            IDomainService<MoneyLock> moneyLockDomain,
            IRealityObjectPaymentAccountRepository accountRepo,
            IChargePeriodRepository periodRepo,
            IDomainService<Wallet> walletDomain,
            IDomainService<MoneyOperation> moneyOperationDomain,
            IDomainService<Transfer> transferDomain,
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
        public override DistributionCode DistributionCode => DistributionCode.TransferContractorDistribution;

        /// <summary>
        /// Идентификатор права доступа
        /// </summary>
        public override string PermissionId => "GkhRegOp.Accounts.SuspenseAccount.EnrollmentTypes.TransferContractor";

        /// <summary>
        /// MoneyOperation Reason
        /// </summary>
        public override string MoneyOperationReason
        {
            get { return "Оплата заявки на перечисление средств подрядчику"; }
        }

        public override string GetApplyTransferReason(TransferCtr transferCtr, MoneyLock mLock)
        {
            return "Оплата заявки {0} по работе {1} ({2})"
                .FormatUsing(
                    transferCtr.PaymentType.GetEnumMeta().Display,
                    transferCtr.TypeWorkCr.Return(x => x.Work).Return(x => x.Name),
                    mLock.SourceName);
        }

        public override string GetUndoTransferReason(TransferCtr transferCtr, MoneyLock mLock)
        {
            return "Отмена оплаты заявки {0} по работе {1} ({2})"
                .FormatUsing(
                    transferCtr.PaymentType.GetEnumMeta().Display,
                    transferCtr.TypeWorkCr.Return(x => x.Work).Return(x => x.Name),
                    mLock.SourceName);
        }
    }
}