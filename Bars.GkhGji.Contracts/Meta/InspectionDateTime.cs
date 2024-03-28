namespace Bars.GkhGji.Contracts.Meta
{
    using System;
    using Bars.B4.Utils;

    public class InspectionDateTime
    {
        [Display("Дата проверки")]
        public DateTime InspectionDate { get; set; }

        [Display("Время начала")]
        public string StartTime { get; set; }

        [Display("Время окончания")]
        public string EndTime { get; set; }

        [Display("Продолжительность")]
        public string Duration { get; set; }
    }
}
