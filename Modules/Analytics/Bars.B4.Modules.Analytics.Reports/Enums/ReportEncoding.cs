namespace Bars.B4.Modules.Analytics.Reports.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Кодировка для отчёта для текстовых форматов(txt, csv)
    /// </summary>
    public enum ReportEncoding
    {
        /// <summary>
        /// Не задано - по умолчанию используется UTF-8
        /// </summary>
        [Display("Не задано")]
        NotSet = 1,

        /// <summary>
        /// UTF-8
        /// </summary>
        [Display("UTF-8")]
        Utf8 = 2,

        /// <summary>
        /// Windows-1251
        /// </summary>
        [Display("Windows-1251")]
        Windows1251 = 3
    }
}
