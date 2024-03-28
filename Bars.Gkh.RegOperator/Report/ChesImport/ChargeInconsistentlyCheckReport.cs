namespace Bars.Gkh.RegOperator.Report.ChesImport
{
    using Bars.B4.Modules.Reports;
    using Bars.Gkh.RegOperator.Properties;

    /// <summary>
    /// Отчет - Проверка некорректных начислений
    /// </summary>
    public class ChargeInconsistentlyCheckReport : ChesImportReport
    {
        /// <inheritdoc />
        public ChargeInconsistentlyCheckReport()
            : base(new ReportTemplateBinary(Resources.ChargeInconsistentlyCheckReport))
        {
        }

        /// <inheritdoc />
        public override string Name => "Проверка некорректных начислений";

        /// <inheritdoc />
        protected override byte[] Template => Resources.ChargeInconsistentlyCheckReport;
    }
}