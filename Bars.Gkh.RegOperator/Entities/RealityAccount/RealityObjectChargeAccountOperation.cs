namespace Bars.Gkh.RegOperator.Entities
{
    using System;
    using B4.Utils.Annotations;

    using Bars.Gkh.Entities;

    /// <summary>
    /// Начисления по счету начисления дома (группировка по периодам)
    /// </summary>
    public class RealityObjectChargeAccountOperation : BaseImportableEntity
    {
        /*
            В карточке счета начислений в периоде дома есть столбцы итого начислено, оплачено и такие же по пени, они формируются так!
            1. Итого начислено: Начислено по БД + ТФ + Пени
            2. Начислено пени:  Начислено Пени
            3. Итого оплачено:  БТ + ТФ
            4. Оплачено пени:   Пени
            Давайте разберемся в этой каше :)
        */

        /// <summary>
        /// .ctor
        /// </summary>
        public RealityObjectChargeAccountOperation()
        {
            
        }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="chargeAccount"></param>
        /// <param name="period"></param>
        public RealityObjectChargeAccountOperation(RealityObjectChargeAccount chargeAccount, ChargePeriod period)
            : this()
        {
            ArgumentChecker.NotNull(chargeAccount, nameof(chargeAccount));
            ArgumentChecker.NotNull(period, nameof(period));

            this.Account = chargeAccount;
            this.Period = period;
            this.Date = period.StartDate;
        }

        /// <summary>
        /// Дата операции
        /// </summary>
        public virtual DateTime Date { get; set; }

        /// <summary>
        /// Период начислений
        /// </summary>
        public virtual ChargePeriod Period { get; set; }

        /// <summary>
        /// Счет начислений дома
        /// </summary>
        public virtual RealityObjectChargeAccount Account { get; set; }

        /// <summary>
        /// Сумма по всем начисленям ЛС по дому за период
        /// </summary>
        public virtual decimal ChargedTotal { get; set; }

        /// <summary>
        /// Входящее сальдо
        /// </summary>
        public virtual decimal SaldoIn { get; set; }

        /// <summary>
        /// Исходящее сальдо
        /// </summary>
        public virtual decimal SaldoOut { get; set; }

        /// <summary>
        /// Оплачено всего
        /// </summary>
        public virtual decimal PaidTotal { get; set; }

        /// <summary>
        /// Начислено пени
        /// </summary>
        public virtual decimal ChargedPenalty { get; set; }

        /// <summary>
        /// Оплачено пени
        /// </summary>
        public virtual decimal PaidPenalty { get; set; }

        /// <summary>
        /// Сумма по операциям установки/изменения сальдо
        /// </summary>
        public virtual decimal BalanceChange { get; set; }

        public virtual void PayTariff(decimal amount, bool updateSaldo = true)
        {
            this.PaidTotal += amount;

            if (updateSaldo)
            {
                this.RecalcSaldoOut();
            }

        }

        public virtual void ChargeByTariff(decimal amount)
        {
            this.ChargedTotal += amount;

            this.RecalcSaldoOut();
        }

        public virtual void PayPenalty(decimal amount, bool updateSaldo = true)
        {
            this.PaidPenalty += amount;

            if (updateSaldo)
            {
                this.RecalcSaldoOut();
            }
        }

        public virtual void ChargeByPenalty(decimal amount)
        {
            this.PaidPenalty -= amount;

            this.RecalcSaldoOut();
        }

        public virtual void ChangeBalance(decimal amount)
        {
            this.ChargedTotal += amount;
            this.BalanceChange += amount;

            this.RecalcSaldoOut();
        }

        public virtual void ApplyCharge(PersonalAccountCharge charge, bool updateSaldo = true)
        {
            this.ChargedPenalty += charge.Penalty + charge.RecalcPenalty;
            this.ChargedTotal += charge.Charge;

            if (updateSaldo)
            {
                this.RecalcSaldoOut();
            }
        }

        public virtual void ApplyCharge(decimal mainCharge, decimal penaltyCharge)
        {
            this.ChargedTotal += mainCharge + penaltyCharge;
            this.ChargedPenalty += penaltyCharge;

            this.RecalcSaldoOut();
        }

        public virtual void RecalcSaldoOut()
        {
            this.SaldoOut = this.SaldoIn + this.ChargedTotal - this.PaidTotal - this.PaidPenalty;
        }
    }
}