namespace Bars.GkhGji.Regions.Khakasia.Entities
{
    using System;

    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    
    /// <summary>
    /// Акт обследования (Хакасия)
    /// </summary>
    public class KhakasiaActSurvey : ActSurvey
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
