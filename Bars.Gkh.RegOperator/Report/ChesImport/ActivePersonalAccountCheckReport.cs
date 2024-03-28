namespace Bars.Gkh.RegOperator.Report.ChesImport
{
    using Bars.B4.Modules.Reports;
    using Bars.Gkh.RegOperator.Properties;

    /// <summary>
    /// Отчет - Количество активных лицевых счетов
    /// </summary>
    public class ActivePersonalAccountCheckReport : ChesImportReport
    {
        /// <inheritdoc />
        public ActivePersonalAccountCheckReport()
            : base(new ReportTemplateBinary(Resources.ActivePersonalAccountCheckReport))
        {
        }

        /// <inheritdoc />
        public override string Name => "Количество активных лицевых счетов";

        /// <inheritdoc />
        protected override byte[] Template => Resources.ActivePersonalAccountCheckReport;
    }
}