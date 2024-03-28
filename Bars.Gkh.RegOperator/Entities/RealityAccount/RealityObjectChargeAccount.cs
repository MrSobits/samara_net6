namespace Bars.Gkh.RegOperator.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    
    using Bars.Gkh.Entities;

    using Newtonsoft.Json;

    /// <summary>
    /// Счет начислений дома
    /// </summary>
    public class RealityObjectChargeAccount : BaseImportableEntity, IRealityObjectAccount
    {
        private IList<RealityObjectChargeAccountOperation> operations;

        /// <summary>
        /// Все начисления (помечено как устаревшее поле) 
        /// </summary>
        [Obsolete("Не актуальное поле, значения собираются через'RealityObjectChargeAccount.Operations.SafeSum(y => y.ChargedTotal)'")]
        public virtual decimal ChargeTotal { get; set; }

        /// <summary>
        /// Все оплаты
        /// </summary>
        public virtual decimal PaidTotal { get; set; }

        /// <summary>
        /// Начисления по счету начисления дома (группировка по периодам)
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<RealityObjectChargeAccountOperation> Operations
        {
            get { return this.operations ?? (this.operations = new List<RealityObjectChargeAccountOperation>()); }
        }

        /// <summary>
        /// Объект недвижимости
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Номер счета
        /// </summary>
        public virtual string AccountNumber { get; set; }

        /// <summary>
        /// Применить оплату
        /// </summary>
        /// <param name="payment"></param>
        /// <param name="isPenalty"></param>
        public virtual void ApplyPayment(decimal payment, bool isPenalty = false)
        {
            var latestChargeOperation = this.GetOpenedPeriodOperation();
            if (latestChargeOperation == null)
            {
                throw new Exception(
                    "В данный момент распределение невозможно, так как в " +
                        "системе существует более чем один открытый период начислений!");
            }

            if (isPenalty)
            {
                latestChargeOperation.PayPenalty(payment);
            }
            else
            {
                latestChargeOperation.PayTariff(payment);
            }             

            this.PaidTotal += payment;
        }

        /// <summary>
        /// Отменить оплату
        /// </summary>
        /// <param name="payment"></param>
        /// <param name="isPenalty"></param>
        public virtual void UndoPayment(decimal payment, bool isPenalty = false)
        {
            var latestChargeOperation = this.GetOpenedPeriodOperation();
            if (isPenalty)
            {
                latestChargeOperation.PayPenalty(-payment);
            }
            else
            {
                latestChargeOperation.PayTariff(-payment);
            }
            
            this.PaidTotal -= payment;
        }

        /// <summary>
        /// Применить изменение по сальдо
        /// </summary>
        /// <param name="saldoByBaseTariffDelta">Изменение сальдо по базовому</param>
        /// <param name="saldoByDecisionTariffDelta">Изменение сальдо по тарифу решения</param>
        /// <param name="saldoByPenaltyDelta">Изменение сальдо по пени</param>
        public virtual void ApplyChangeSaldo(decimal saldoByBaseTariffDelta, decimal saldoByDecisionTariffDelta, decimal saldoByPenaltyDelta)
        {
            var latestChargeOperation = this.GetOpenedPeriodOperation();
            latestChargeOperation.ApplyCharge(saldoByBaseTariffDelta + saldoByDecisionTariffDelta, saldoByPenaltyDelta);
        }

        /// <summary>
        /// Отмена начисления 
        /// </summary>
        public virtual void UndoCharge(decimal tariffSum, decimal penalty)
        {
            var latestChargeOperation = this.GetOpenedPeriodOperation();
            latestChargeOperation.ApplyCharge(-tariffSum, -penalty);
        }

        private RealityObjectChargeAccountOperation GetOpenedPeriodOperation()
        {
            if (!this.Operations.Any())
            {
                throw new InvalidOperationException("Отсутствуют операции по счету начислений дома. Создайте их в меню 'Администрирование -> Действия'");
            }

            var latestChargeOperation = this.Operations.SingleOrDefault(x => !x.Period.IsClosed);

            return latestChargeOperation;
        }
    }
}