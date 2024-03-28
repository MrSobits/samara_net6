namespace Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations
{
    using System.Collections.Generic;

    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Enums;
    using ValueObjects;
    
    /// <summary>
    /// Отмена начислений из импорта
    /// </summary>
    public class ImportCancelChargeSource : ChargeOperationBase
    {
        private readonly IList<CancelCharge> cancelCharges;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="period">Период</param>
        public ImportCancelChargeSource(ChargePeriod period) : base(period)
        {
            this.ChargeSource = TypeChargeSource.ImportCancelCharge;
            this.cancelCharges = new List<CancelCharge>();
        }

        /// <summary>
        /// .ctor
        /// </summary>
        protected ImportCancelChargeSource()
        {
        }

        /// <summary>
        /// Детализация отмены (по лс и периодам)
        /// </summary>
        public virtual IEnumerable<CancelCharge> CancelCharges => this.cancelCharges;


        /// <summary>
        /// Добавление отмен начислений
        /// </summary>
        public virtual void AddCancelCharge(BasePersonalAccount account, ChargePeriod cancelPeriod,
            decimal baseTariffSum, decimal decisionTariffSum, decimal penaltySum)
        {
            if (baseTariffSum != 0)
            {
                this.cancelCharges.Add(
                    new CancelCharge(
                        this,
                        account,
                        cancelPeriod,
                        baseTariffSum,
                        CancelType.BaseTariffCharge));
            }

            if (decisionTariffSum != 0)
            {
                this.cancelCharges.Add(
                    new CancelCharge(
                        this,
                        account,
                        cancelPeriod,
                        decisionTariffSum,
                        CancelType.DecisionTariffCharge));
            }

            if (penaltySum != 0)
            {
                this.cancelCharges.Add(
                    new CancelCharge(
                        this, 
                        account,
                        cancelPeriod, 
                        penaltySum, 
                        CancelType.Penalty));
            }
        }

        /// <summary>
        /// Создать операцию по передвижению денег
        /// </summary>
        public override MoneyOperation CreateOperation(ChargePeriod period)
        {
            var operation = base.CreateOperation(period);
            operation.Reason = "Массовая отмена начислений";

            return operation;
        }
    }
}
