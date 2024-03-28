namespace Bars.B4.Modules.Analytics.Reports.Utils
{
    using System.IO;

    /// <summary>
    /// Результат формирования отчета
    /// </summary>
    public class ReportResult : BaseDataResult
    {
        /// <summary>
        /// Поток
        /// </summary>
        public Stream ReportStream { get; set; }

        /// <summary>
        /// Наименование файла
        /// </summary>
        public string FileName { get; set; }
    }
}