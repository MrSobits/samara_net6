namespace Bars.GkhGji.Regions.Zabaykalye.Entities
{
    using System;

    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    
    /// <summary>
    /// Акт обследования переделан под Саху
    /// </summary>
    public class ZabaykalyeActSurvey : ActSurvey
    {
        /// <summary>
        /// Дата проведения
        /// </summary>
        public virtual DateTime? DateOf { get; set; }

        /// <summary>
        /// Время начала
        /// </summary>
        public virtual DateTime? TimeStart { get; set; }

        /// <summary>
        /// Время окончания
        /// </summary>
        public virtual DateTime? TimeEnd { get; set; }

        /// <summary>
        /// Заключение вынесено
        /// </summary>
        public virtual YesNo ConclusionIssued { get; set; }

    }
}
