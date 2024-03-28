namespace Bars.GisIntegration.Base.Entities.Services
{
    using System;

    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// План работ по перечню работ/услуг
    /// </summary>
    public class WorkPlanItem : BaseRisEntity
    {
        /// <summary>
        /// План по перечню работ/услуг
        /// </summary>
        public virtual WorkingPlan WorkingPlan { get; set; }

        /// <summary>
        /// Работа/услуга перечня
        /// </summary>
        public virtual WorkListItem WorkListItem { get; set; }

        /// <summary>
        /// Год
        /// </summary>
        public virtual short Year { get; set; }

        /// <summary>
        /// Месяц
        /// </summary>
        public virtual int Month { get; set; }

        /// <summary>
        /// Дата начала работ по плану
        /// </summary>
        public virtual DateTime WorkDate { get; set; }

        /// <summary>
        /// Количество работ
        /// </summary>
        public virtual int WorkCount { get; set; }
    }
}