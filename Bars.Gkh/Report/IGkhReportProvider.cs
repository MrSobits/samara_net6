namespace Bars.Gkh.Report
{
    using System.IO;

    using B4.Modules.Reports;

    /// <summary>
    /// ��������� ��� 
    /// </summary>
    public interface IGkhReportProvider : IReportProvider
    {
        void GenerateReport(IBaseReport report, Stream result, IReportGenerator reportGenerator, ReportParams reportParams);
    }
}