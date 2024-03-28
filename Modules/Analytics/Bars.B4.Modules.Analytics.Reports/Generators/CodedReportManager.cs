namespace Bars.B4.Modules.Analytics.Reports.Generators
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Analytics.Reports.Domain;
    using Bars.B4.Modules.Analytics.Reports.Entities;
    using Bars.B4.Modules.Analytics.Reports.Enums;

    /// <summary>
    /// Менеджер отчета
    /// </summary>
    public class CodedReportManager : StimulReportGenerator, ICodedReportManager
    {
        private readonly IRepository<ReportCustom> reportCustomRepo;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="reportCustomRepo">Репозиторий для <see cref="ReportCustom"/></param>
        public CodedReportManager(IRepository<ReportCustom> reportCustomRepo)
        {
            this.reportCustomRepo = reportCustomRepo;
        }

        /// <inheritdoc />
        public Stream ExtractTemplateForEdit(ICodedReport codedReport, bool original = false)
        {
            var template = this.InternalExtractTemplate(codedReport, original);

            var report = new CustomReport(codedReport.GetDataSources(),
                codedReport.GetParams(),
                codedReport.Key,
                codedReport.Name,
                template);

            var extraParams = new Dictionary<string, object>
            {
                { "ConnectionString", ApplicationContext.Current.Configuration.ConnString }
            };

            var container = ApplicationContext.Current.Container;
            var remoteReportService = container.Resolve<IRemoteReportService>();

            using (container.Using(remoteReportService))
            {
                return remoteReportService.GetTemplateWithMeta(report, new BaseParams(), extraParams);
            }
        }

        /// <inheritdoc />
        public Stream GetEmptyTemplate(ICodedReport codedReport)
        {
            var templateService = new EmptyTemplateService();
            return templateService.GetTemplateWithMeta(codedReport.GetDataSources());
        }

        /// <inheritdoc />
        public Stream GenerateReport(ICodedReport codedReport, BaseParams baseParams, ReportPrintFormat printFormat)
        {
            var template = this.InternalExtractTemplate(codedReport);
            return this.Generate(
                codedReport,
                template,
                baseParams,
                printFormat,
                new Dictionary<string, object>
                    {
                        { "ExportSettings", codedReport.GetExportSettings(printFormat) }
                    });
        }

        private Stream InternalExtractTemplate(ICodedReport codedReport, bool original = false)
        {
            if (original)
            {
                return codedReport.GetTemplate();
            }

            var customization = this.reportCustomRepo.FirstOrDefault(x => x.CodedReportKey == codedReport.Key);
            return customization != null ? new MemoryStream(customization.Template) : codedReport.GetTemplate();
        }
    }
}