namespace Bars.Gkh.Report
{
    using System;
    using System.IO;
    using B4.Modules.Reports;

    /// <summary>
    /// The report provider.
    /// </summary>
    public class GkhReportProvider : IGkhReportProvider
    {
        /// <summary>
        ///   The report generator.
        /// </summary>
        private IReportGenerator reportGenerator;
        private ReportParams reportParams;

        public GkhReportProvider(IReportGenerator reportGenerator)
        {
            this.reportGenerator = reportGenerator;
        }

        #region IReportProvider Members

        public void GenerateReport(IBaseReport report, Stream result, IReportGenerator reportGenerator, ReportParams reportParams)
        {
            this.reportGenerator = reportGenerator;
            this.reportParams = reportParams;

            GenerateReport(report, result);
        }

        public void GenerateReport(IBaseReport report, string filePath)
        {
            var result = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);

            GenerateReport(report, result);
        }

        public void GenerateReport(IBaseReport report, Stream result)
        {
            Stream reportTemplate = report.GetTemplate();

            if (reportTemplate == null || !reportTemplate.CanRead)
            {
                throw new ReportProviderException("Невозможно прочитать файл шаблона");
            }

            this.reportGenerator.Open(reportTemplate);

            this.reportGenerator.Generate(result, this.reportParams);

            result.Seek(0, SeekOrigin.Begin);
        }
        #endregion

    }
}