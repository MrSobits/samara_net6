namespace Bars.B4.Modules.Analytics.Reports.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип отчета
    /// </summary>
    public enum ReportType
    {
        /// <summary>
        /// Отчет в коде (старый)
        /// </summary>
        [Display("Отчет в коде (старый)")]
        PrintForm = 1,

        /// <summary>
        /// Хранимый отчет (новый)
        /// </summary>
        [Display("Хранимый отчет (новый)")]
        StoredReport = 2
    }
}
