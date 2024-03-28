namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.B4.DataAccess;

    /// <summary>
    /// Дата и время проведения проверки
    /// </summary>
    public class PreventiveVisitPeriod : BaseEntity
    {
        /// <summary>
        /// Акт проверки
        /// </summary>
        public virtual PreventiveVisit PreventiveVisit { get; set; }

        /// <summary>
        /// Дата
        /// </summary>
        public virtual DateTime? DateCheck { get; set; }

        /// <summary>
        /// Дата начала
        /// </summary>
        public virtual DateTime? DateStart { get; set; }

        /// <summary>
        /// Дата окончания 
        /// </summary>
        public virtual DateTime? DateEnd { get; set; }

        /// <summary>
        /// Время начала
        /// </summary>
        public virtual string TimeStart { get; set; }

        /// <summary>
        /// Время окончания 
        /// </summary>
        public virtual string TimeEnd { get; set; }
    }
}