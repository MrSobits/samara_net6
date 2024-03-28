namespace Bars.GkhCr.Entities
{
    using Gkh.Entities;

    /// <summary>
    /// Средства источника финансирования
    /// </summary>
    public class FinanceSourceResource : BaseGkhEntity
    {
        /// <summary>
        /// Объект капитального ремонта
        /// </summary>
        public virtual ObjectCr ObjectCr { get; set; }

        /// <summary>
        /// Разрез финансирования
        /// </summary>
        public virtual FinanceSource FinanceSource { get; set; }

        /// <summary>
        /// Бюджет МО
        /// </summary>
        public virtual decimal? BudgetMu { get; set; }

        /// <summary>
        /// Бюджет субъекта
        /// </summary>
        public virtual decimal? BudgetSubject { get; set; }

        /// <summary>
        /// Средства собственника
        /// </summary>
        public virtual decimal? OwnerResource { get; set; }

        /// <summary>
        /// Средства фонда
        /// </summary>
        public virtual decimal? FundResource { get; set; }

        /// <summary>
        /// Поступило из Бюджет МО
        /// </summary>
        public virtual decimal? BudgetMuIncome { get; set; }

        /// <summary>
        /// Поступило из Бюджет субъекта
        /// </summary>
        public virtual decimal? BudgetSubjectIncome { get; set; }

        /// <summary>
        /// Поступило из Средства фонда
        /// </summary>
        public virtual decimal? FundResourceIncome { get; set; }

        /// <summary>
        /// Вид работ
        /// </summary>
        public virtual TypeWorkCr TypeWorkCr { get; set; }

        /// <summary>
        /// Год
        /// </summary>
        public virtual int? Year { get; set; }
    }
}
