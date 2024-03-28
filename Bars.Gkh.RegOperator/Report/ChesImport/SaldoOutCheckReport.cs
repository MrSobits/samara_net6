namespace Bars.Gkh.RegOperator.Report.ChesImport
{
    using Bars.B4.Modules.Reports;
    using Bars.Gkh.RegOperator.Properties;

    /// <summary>
    /// Отчет - Несоответствие исходящего сальдо
    /// </summary>
    public class SaldoOutCheckReport : ChesImportReport
    {
        /// <inheritdoc />
        public SaldoOutCheckReport()
            : base(new ReportTemplateBinary(Resources.SaldoOutCheckReport))
        {
        }

        /// <inheritdoc />
        public override string Name => "Несоответствие исходящего сальдо";

        /// <inheritdoc />
        protected override byte[] Template => Resources.SaldoOutCheckReport;
    }
}