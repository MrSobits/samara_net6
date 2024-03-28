namespace Bars.Gkh.Entities
{
    using System;

    /// <summary>
    /// График погашения займа подрядчика
    /// </summary>
    public class BuilderLoanRepayment : BaseGkhEntity
    {
        /// <summary>
        /// Займ подрядчика
        /// </summary>
        public virtual BuilderLoan BuilderLoan { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Сумма погашения
        /// </summary>
        public virtual decimal? RepaymentAmount { get; set; }

        /// <summary>
        /// Дата погашения
        /// </summary>
        public virtual DateTime? RepaymentDate { get; set; }
    }
}
