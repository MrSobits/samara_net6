namespace Bars.GkhGji.Regions.Tatarstan.Entities.Dict.BudgetClassificationCode
{
    using System;

    using Bars.B4.DataAccess;

    /// <summary>
    /// Код бюджетной классификации.
    /// </summary>
    public class BudgetClassificationCode : BaseEntity
    {
        /// <summary>
        /// КБК
        /// </summary>
        public virtual string Kbk { get; set; }

        /// <summary>
        /// Дата начала действия
        /// </summary>
        public virtual DateTime? StartDate { get; set; }

        /// <summary>
        /// Дата окончания действия
        /// </summary>
        public virtual DateTime? EndDate { get; set; }
    }
}
