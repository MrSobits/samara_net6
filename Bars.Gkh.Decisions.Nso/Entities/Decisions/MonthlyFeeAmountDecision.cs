namespace Bars.Gkh.Decisions.Nso.Entities
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///     Размер ежемесячного взноса на КР
    /// </summary>
    public class MonthlyFeeAmountDecision : UltimateDecision
    {
        /// <summary>
        ///     Текущие взносы
        /// </summary>
        public virtual List<PeriodMonthlyFee> Decision { get; set; }

        /// <summary>
        ///     Минимальные взносы
        /// </summary>
        public virtual List<PeriodMonthlyFee> Defaults { get; set; }

        /// <summary>
        ///     Создает экземпляр.
        /// </summary>
        public MonthlyFeeAmountDecision()
        {
            Defaults = new List<PeriodMonthlyFee>();
            Decision = new List<PeriodMonthlyFee>();
        }
    }

    /// <summary>
    ///     Месячный взнос
    /// </summary>
    public class PeriodMonthlyFee
    {
        /// <summary>
        ///     Сумма
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        ///     От
        /// </summary>
        public DateTime? From { get; set; }

        /// <summary>
        ///     До
        /// </summary>
        public DateTime? To { get; set; }

        /// <summary>
        ///     Идентификатор
        /// </summary>
        public string Guid { get; set; }
    }
}