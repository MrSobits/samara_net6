namespace Bars.Gkh.RegOperator.Report.ChesImport
{
    using Bars.B4.Modules.Reports;
    using Bars.Gkh.RegOperator.Properties;

    /// <summary>
    /// Отчет - Проверка невалидных начислений по МКД
    /// </summary>
    public class InvalidChargesMkdCheckReport : ChesImportReport
    {
        /// <inheritdoc />
        public InvalidChargesMkdCheckReport()
            : base(new ReportTemplateBinary(Resources.InvalidChargesMkdCheckReport))
        {
        }

        /// <inheritdoc />
        public override string Name => "Проверка невалидных начислений по МКД";

        /// <inheritdoc />
        protected override byte[] Template => Resources.InvalidChargesMkdCheckReport;
    }
}