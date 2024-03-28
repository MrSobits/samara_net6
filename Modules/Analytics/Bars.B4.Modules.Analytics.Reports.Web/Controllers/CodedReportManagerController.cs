namespace Bars.B4.Modules.Analytics.Reports.Web.Controllers
{
    using System.IO;
    using Bars.B4;
    using Bars.B4.Modules.Analytics.Reports.Domain;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Analytics.Reports.Generators;
    using IoC;

    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Контроллер для <see cref="CodedReportManager"/>
    /// </summary>
    public class CodedReportManagerController : BaseController
    {
        /// <summary>
        /// Скачать шаблон
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult DownloadTemplate(BaseParams baseParams)
        {
            var codedReport =
                this.Container.Resolve<ICodedReportService>().Get(baseParams.Params.GetAs<string>("CodedReportKey", ignoreCase: true));

            var reportManager = this.Container.Resolve<ICodedReportManager>();

            using (this.Container.Using(reportManager))
            {
                var template = reportManager.ExtractTemplateForEdit(codedReport, baseParams.Params.GetAs<bool>("Original", ignoreCase: true));
                template.Seek(0, SeekOrigin.Begin);
                return this.File(template, System.Net.Mime.MediaTypeNames.Application.Octet, string.Format("{0}.mrt", codedReport.Key));
            }
        }

        /// <summary>
        /// Скачать пустой шаблон
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult DownloadEmptyTemplate(BaseParams baseParams)
        {
            var codedReport = this.Container.Resolve<ICodedReportService>().Get(baseParams.Params.GetAs<string>("CodedReportKey", ignoreCase: true));

            var reportManager = this.Container.Resolve<ICodedReportManager>();

            using (this.Container.Using(reportManager))
            {
                var template = reportManager.GetEmptyTemplate(codedReport);
                template.Seek(0, SeekOrigin.Begin);
                return this.File(template, System.Net.Mime.MediaTypeNames.Application.Octet, string.Format("Шаблон {0}.mrt", codedReport.Key));
            }
        }

        /// <summary>
        /// Сгенерировать отчет
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult Generate(BaseParams baseParams)
        {
            var codedReport =
                this.Container.Resolve<ICodedReportService>().Get(baseParams.Params.GetAs<string>("CodedReportKey", ignoreCase: true));
            var extension = baseParams.Params.GetAs("Extension", ReportPrintFormat.docx, true);

            var reportManager = this.Container.Resolve<ICodedReportManager>();

            using (this.Container.Using(reportManager))
            {
                var template = reportManager.GenerateReport(codedReport, baseParams, extension);
                template.Seek(0, SeekOrigin.Begin);
                return this.File(template, System.Net.Mime.MediaTypeNames.Application.Octet, string.Format("{0}.docx", codedReport.Key));
            }
        }
    }
}
