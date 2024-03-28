namespace Bars.Gkh.Regions.Tatarstan.Reports
{
    using Bars.B4.Modules.DataExport;
    using Bars.B4.Modules.Reports;

    /// <summary>
    /// Формирование лога в формате xlsx
    /// </summary>
    public class LogReport : DataExportReport
    {
        /// <inheritdoc />
        public LogReport()
            : base(new ReportTemplateBinary(Properties.Resources.CourtOrderInfoImportLogPrintForm))
        {
        }

        /// <inheritdoc />
        public override void PrepareReport(ReportParams reportParams)
        {
        }

        /// <inheritdoc />
        public override string Name => string.Empty;
    }
}