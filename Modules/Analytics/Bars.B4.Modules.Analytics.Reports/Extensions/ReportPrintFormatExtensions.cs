namespace Bars.B4.Modules.Analytics.Reports.Extensions
{
    using Bars.B4.Modules.Analytics.Reports.Enums;

    /// <summary>
    /// Расширение для формата отчета
    /// </summary>
    public static class ReportPrintFormatExtensions
    {
        public static StiExportFormat ExportFormat(this ReportPrintFormat printFormat)
        {
            return  printFormat == ReportPrintFormat.xlsx ? StiExportFormat.Excel2007 :
                    printFormat == ReportPrintFormat.docx ? StiExportFormat.Word2007 :
                    printFormat == ReportPrintFormat.pdf ? StiExportFormat.Pdf :
                    printFormat == ReportPrintFormat.xls ? StiExportFormat.Excel :
                    printFormat == ReportPrintFormat.csv ? StiExportFormat.Csv :
                    printFormat == ReportPrintFormat.text ? StiExportFormat.Text :
                    printFormat == ReportPrintFormat.xps ? StiExportFormat.Xps :
                    printFormat == ReportPrintFormat.odt ? StiExportFormat.Odt :
                    StiExportFormat.None;
        }

        public static string Extension(this ReportPrintFormat printFormat)
        {
            return  printFormat == ReportPrintFormat.xlsx ? "xlsx" :
                    printFormat == ReportPrintFormat.docx ? "docx" :
                    printFormat == ReportPrintFormat.pdf ? "pdf" :
                    printFormat == ReportPrintFormat.xls ? "xls" :
                    printFormat == ReportPrintFormat.csv ? "csv" :
                    printFormat == ReportPrintFormat.text ? "txt" :
                    printFormat == ReportPrintFormat.xps ? "xps" :
                    printFormat == ReportPrintFormat.zip ? "zip" :
                    printFormat == ReportPrintFormat.odt ? "odt" :
                    string.Empty;
        }
    }
}
