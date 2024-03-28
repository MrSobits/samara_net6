namespace Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations
{
    using System.Collections.Generic;

    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Domain.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;
    using ValueObjects;
    
    /// <summary>
    /// Отмена начислений
    /// </summary>
    public class CancelChargeSource : ChargeOperationBase
    {
        private readonly IList<CancelCharge> cancelCharges;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="period">Период</param>
        public CancelChargeSource(ChargePeriod period) : base(period)
        {
            this.ChargeSource = TypeChargeSource.CancelCharge;
            this.cancelCharges = new List<CancelCharge>();
        }

        /// <summary>
        /// .ctor
        /// </summary>
        protected CancelChargeSource()
        {
        }

        /// <summary>
        /// Детализация отмены (по лс и периодам)
        /// </summary>
        public virtual IEnumerable<CancelCharge> CancelCharges => this.cancelCharges;


        /// <summary>
        /// Добавление отмен начислений
        /// </summary>
        /// <param name="accountCancelInfo">Информация по лс</param>
        public virtual void AddCancelCharge(PersonalAccountCancelChargeInfo accountCancelInfo)
        {
            if (accountCancelInfo.BaseTariffSum != 0)
            {
                this.cancelCharges.Add(
                    new CancelCharge(
                        this,
                        accountCancelInfo.Account,
                        accountCancelInfo.Period,
                        accountCancelInfo.BaseTariffSum,
                        CancelType.BaseTariffCharge));
            }

            if (accountCancelInfo.DecisionTariffSum != 0)
            {
                this.cancelCharges.Add(
                    new CancelCharge(
                        this,
                        accountCancelInfo.Account,
                        accountCancelInfo.Period,
                        accountCancelInfo.DecisionTariffSum,
                        CancelType.DecisionTariffCharge));
            }

            if (accountCancelInfo.PenaltySum != 0)
            {
                this.cancelCharges.Add(
                    new CancelCharge(
                        this, 
                        accountCancelInfo.Account, 
                        accountCancelInfo.Period, 
                        accountCancelInfo.PenaltySum, 
                        CancelType.Penalty));
            }

            if (accountCancelInfo.BaseTariffChange != 0)
            {
                this.cancelCharges.Add(
                    new CancelCharge(
                        this,
                        accountCancelInfo.Account,
                        accountCancelInfo.Period,
                        accountCancelInfo.BaseTariffChange,
                        CancelType.BaseTariffChange));
            }

            if (accountCancelInfo.DecisionTariffChange != 0)
            {
                this.cancelCharges.Add(
                    new CancelCharge(
                        this,
                        accountCancelInfo.Account,
                        accountCancelInfo.Period,
                        accountCancelInfo.DecisionTariffChange,
                        CancelType.DecisionTariffChange));
            }

            if (accountCancelInfo.PenaltyChange != 0)
            {
                this.cancelCharges.Add(
                    new CancelCharge(
                        this,
                        accountCancelInfo.Account,
                        accountCancelInfo.Period,
                        accountCancelInfo.PenaltyChange,
                        CancelType.PenaltyChange));
            }
        }

        /// <summary>
        /// Создать операцию по передвижению денег
        /// </summary>
        /// <returns><see cref="MoneyOperation"/></returns>
        public override MoneyOperation CreateOperation(ChargePeriod period)
        {
            var operation = base.CreateOperation(period);
            operation.Reason = "Массовая отмена начислений";

            return operation;
        }
    }
}
