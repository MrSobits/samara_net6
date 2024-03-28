namespace Bars.B4.Modules.Analytics.Reports.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Extensions;
    using IoC;
    using Entities;
    using Enums;
    using Generators;
    using ViewModels;
    using B4.Utils;

    using Bars.B4.DataAccess;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// 
    /// </summary>
    public class ExternalReportController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult Generate(BaseParams baseParams)
        {
            var reportDomain = Container.Resolve<IDomainService<StoredReport>>();
            var generator = Container.Resolve<IReportGenerator>();
            var logManager = this.Container.Resolve<ILogger>();

            using (Container.Using(reportDomain, generator))
            {
                var reportId = baseParams.Params.GetAs<long>("reportId", ignoreCase: true);
                var token = baseParams.Params.GetAs<string>("token", ignoreCase: true);
                var format = baseParams.Params.GetAs("format", ignoreCase: true, defaultValue: ReportPrintFormat.xls);

                var report = reportDomain.FirstOrDefault(x => x.Id == reportId);
                if (report == null || !MD5.GetHashString64(report.Id + StoredReportViewModel.Salt).Equals(token))
                {
                    return JsFailure("Отчет по указанной ссылке не обнаружен. Попробуйте сформировать внешнюю ссылку на отчет повторно.");
                }

                try
                {
                    var file = generator.Generate(
                        report,
                        report.GetTemplate(),
                        baseParams,
                        format,
                        new Dictionary<string, object>
                            {
                                { "ExportSettings", report.GetExportSettings(format) },
                                { "UseTemplateConnectionString", report.UseTemplateConnectionString }
                            });
                    file.Seek(0, SeekOrigin.Begin);

                    var fileNameEncode =
                        System.Web.HttpUtility.UrlEncode(string.Format("{0}.{1}", report.Name, format.Extension()));

                    return File(file, System.Net.Mime.MediaTypeNames.Application.Octet, fileNameEncode);
                }
                catch (Exception e)
                {
                    logManager.LogError(e, "Ошибка формирования отчета");
                    return JsFailure("Ошибка формирования отчета");
                }
            }
        }
    }
}
