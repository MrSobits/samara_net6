namespace Bars.Gkh.Gis.Reports.Reports
{
    using System.IO;
    using B4.Modules.Reports;

    using Bars.B4;
    using Bars.B4.Modules.Analytics.Reports.Enums;

    /// <summary>
    /// Так как используется ReportPanel которая не поддерживает выбор формата, то необходимо задавать его через реализации
    ///     классов-наследников StimulReport
    /// </summary>
    public abstract class StimulReportDynamicExcel : StimulReport, IGeneratedPrintForm
    {
        public new void Generate(Stream result, ReportParams reportParams)
        {
            ExportFormat = StiExportFormat.Excel2007;
            base.Generate(result, reportParams);
        }
    }
}
