namespace Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations
{
    using System;
    using System.Collections.Generic;

    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.DomainModelServices.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Источник "Разделение лицевого счета"
    /// </summary>
    public class SplitAccountSource : ChargeOperationBase
    {
        private MoneyOperation operation;
        private readonly IList<SplitAccountDetail> splitAccountDetails;

        /// <summary>
        /// Сливаемый лицевой счет
        /// </summary>
        public virtual BasePersonalAccount SourceAccount { get; set; }

        /// <summary>
        /// Детализация операции
        /// </summary>
        public virtual IEnumerable<SplitAccountDetail> SplitAccountDetails => this.splitAccountDetails;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="sourceAccount">Аккаунт-источник</param>
        /// <param name="period">период операции</param>
        /// <param name="splitDate">Дата разделения</param>
        public SplitAccountSource(BasePersonalAccount sourceAccount, ChargePeriod period, DateTime splitDate) : base(period)
        {
            this.ChargeSource = TypeChargeSource.SplitAccount;
            this.SourceAccount = sourceAccount;
            this.OperationDate = splitDate;

            this.splitAccountDetails = new List<SplitAccountDetail>();
        }

        /// <summary>
        /// .ctor
        /// </summary>
        protected SplitAccountSource()
        {
        }

        public virtual IList<Transfer> SplitTo(BasePersonalAccount targetAccount, decimal baseTariff, decimal decisionTariff, decimal penalty)
        {
            var transfers = new List<Transfer>();

            // base tariff
            var amount = baseTariff;
            var source = amount < 0 ? (ITransferParty)new PaymentOriginatorWrapper(this, this.operation) : this;

            transfers.AddRange(this.SourceAccount.MoveMoney(
                this.SourceAccount.BaseTariffWallet,
                targetAccount,
                targetAccount.BaseTariffWallet,
                this.operation,
                source,
                amount,
                this.OperationDate));

            if (amount != 0)
            {
                this.splitAccountDetails.Add(new SplitAccountDetail
                {
                    Account = targetAccount,
                    Operation = this,
                    Amount = amount,
                    WalletType = WalletType.BaseTariffWallet
                });
            }

            this.SourceAccount.MoveBaseTariffCharge(targetAccount, amount);

            // decision tariff
            amount = decisionTariff;
            source = amount < 0 ? (ITransferParty)new PaymentOriginatorWrapper(this, this.operation) : this;
            transfers.AddRange(this.SourceAccount.MoveMoney(
                this.SourceAccount.DecisionTariffWallet,
                targetAccount,
                targetAccount.DecisionTariffWallet,
                this.operation,
                source,
                amount,
                this.OperationDate));

            if (amount != 0)
            {
                this.splitAccountDetails.Add(new SplitAccountDetail
                {
                    Account = targetAccount,
                    Operation = this,
                    Amount = amount,
                    WalletType = WalletType.DecisionTariffWallet
                });
            }

            this.SourceAccount.MoveDecisionTariffCharge(targetAccount, amount);

            // penalty
            amount = penalty;
            source = amount < 0 ? (ITransferParty)new PaymentOriginatorWrapper(this, this.operation) : this;
            transfers.AddRange(this.SourceAccount.MoveMoney(
                this.SourceAccount.PenaltyWallet,
                targetAccount,
                targetAccount.PenaltyWallet,
                this.operation,
                source,
                amount,
                this.OperationDate));

            if (amount != 0)
            {
                this.splitAccountDetails.Add(new SplitAccountDetail
                {
                    Account = targetAccount,
                    Operation = this,
                    Amount = amount,
                    WalletType = WalletType.PenaltyWallet
                });
            }

            this.SourceAccount.MovePenaltyCharge(targetAccount, amount);

            return transfers;
        }

        /// <summary>
        /// Создать операцию по передвижению денег
        /// </summary>
        /// <returns><see cref="MoneyOperation"/></returns>
        public override MoneyOperation CreateOperation(ChargePeriod period)
        {
            this.operation = base.CreateOperation(period);

            // пока так, потому что кое-где мы завязаны на MoneyOperation.Reason
            this.operation.Reason = "Перенос долга при слиянии";

            return this.operation;
        }
    }
}