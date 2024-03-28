namespace Bars.B4.Modules.Analytics.Reports.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Формат отчета
    /// </summary>
    public enum ReportPrintFormat
    {
        [Display("Microsoft Word 2007 (docx)")]
        docx,

        [Display("Microsoft Excel 1995 (xls)")]
        xls,

        [Display("Microsoft Excel 2007 (xlsx)")]
        xlsx,

        [Display("Portable Document Format (pdf)")]
        pdf,

        [Display("Linux office (odt)")]
        odt,

        [Display("Comma Separated Values (csv)")]
        csv,

        [Display("Text file (txt)")]
        text,

        [Display("XML Paper Specification (xps)")]
        xps,

        [Display("ZIP archive (zip)")]
        zip
    }
}
