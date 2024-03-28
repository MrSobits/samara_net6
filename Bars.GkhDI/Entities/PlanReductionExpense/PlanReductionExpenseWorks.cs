namespace Bars.GkhDi.Entities
{
    using System;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Работы по плану мер по снижению расходов
    /// </summary>
    public class PlanReductionExpenseWorks : BaseGkhEntity
    {
        /// <summary>
        /// План мер по снижению расходов
        /// </summary>
        public virtual PlanReductionExpense PlanReductionExpense { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Срок выполнения
        /// </summary>
        public virtual DateTime? DateComplete { get; set; }

        /// <summary>
        /// Предполагаемое снижение расходов
        /// </summary>
        public virtual decimal? PlannedReductionExpense { get; set; }

        /// <summary>
        /// Фактическое снижение расходов
        /// </summary>
        public virtual decimal? FactedReductionExpense { get; set; }

        /// <summary>
        /// Причина отклонения
        /// </summary>
        public virtual string ReasonRejection { get; set; }
    }
}
