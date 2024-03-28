namespace Bars.Gkh.RegOperator.Controllers.PersonalAccount.PayDoc
{
    using System.IO;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Analytics.Reports;
    using Bars.B4.Modules.Analytics.Reports.Domain;
    using Bars.Gkh.RegOperator.CodedReports;
    using Bars.Gkh.RegOperator.CodedReports.PayDoc;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.Report.ReportManager;

    /// <summary>
    /// Контроллер для <see cref="PaymentDocReportManager"/>
    /// </summary>
    public class PaymentDocReportManagerController : BaseController
    {
        /// <summary>
        /// Скачать шаблон
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns>Результат</returns>
        public ActionResult DownloadTemplate(BaseParams baseParams)
        {
            var templateRepo = this.Container.ResolveRepository<PaymentDocumentTemplate>();

            try
            {
                var reportManager = new PaymentDocReportManager(templateRepo);
                var codedReport = this.Container.Resolve<ICodedReportService>()
                    .Get(baseParams.Params.GetAs<string>("templateCode", ignoreCase: true));
                var periodId = baseParams.Params.GetAs<long>("periodId");
                var original = baseParams.Params.GetAs<bool>("original", ignoreCase: true);

                var template = reportManager.ExtractTemplate(codedReport, periodId, original);
                template.Seek(0, SeekOrigin.Begin);

                return this.File(template, System.Net.Mime.MediaTypeNames.Application.Octet, string.Format("{0}.mrt", codedReport.Key));
            }
            finally
            {
                this.Container.Release(templateRepo);
            }
        }

        /// <summary>
        /// Список шаблонов, хранящихся в сущности PaymentDocTemplate
        /// </summary>
        public ActionResult TemplateList(StoreLoadParams storeParams)
        {
            var reports = new ICodedReport[]
            {
                new BaseInvoiceReport(null),
                new InvoiceAndActReport(null),
                new InvoiceRegistryAndActReport(null, null)
            };
            
            return new JsonListResult(reports, reports.Length);
        }
    }
}