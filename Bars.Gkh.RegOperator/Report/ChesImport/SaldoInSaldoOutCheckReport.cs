namespace Bars.Gkh.RegOperator.Report.ChesImport
{
    using Bars.B4.Modules.Reports;
    using Bars.Gkh.RegOperator.Properties;

    /// <summary>
    /// Отчет - Входящее сальдо не равно исходящему
    /// </summary>
    public class SaldoInSaldoOutCheckReport : ChesImportReport
    {
        /// <inheritdoc />
        public SaldoInSaldoOutCheckReport()
            : base(new ReportTemplateBinary(Resources.SaldoInSaldoOutCheckReport))
        {
        }

        /// <inheritdoc />
        public override string Name => "Входящее сальдо не равно исходящему";

        /// <inheritdoc />
        protected override byte[] Template => Resources.SaldoInSaldoOutCheckReport;
    }
}