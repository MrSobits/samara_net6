namespace Bars.Gkh.RegOperator.Report.ChesImport
{
    using Bars.B4.Modules.Reports;
    using Bars.Gkh.RegOperator.Properties;

    /// <summary>
    /// Отчет - Изменения данных по лицевым счетам
    /// </summary>
    public class PersonalAccountChangeCheckReport : ChesImportReport
    {
        /// <inheritdoc />
        public PersonalAccountChangeCheckReport()
            : base(new ReportTemplateBinary(Resources.PersonalAccountChangeCheckReport))
        {
        }

        /// <inheritdoc />
        public override string Name => "Изменения данных по лицевым счетам";

        /// <inheritdoc />
        protected override byte[] Template => Resources.PersonalAccountChangeCheckReport;
    }
}