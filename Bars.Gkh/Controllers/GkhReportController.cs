namespace Bars.Gkh.Controllers
{
    using System;
    using System.Net;

    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Utils.Web;
    using Bars.Gkh.Report;

    using Microsoft.AspNetCore.Http;

    public class GkhReportController : BaseController
    {
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public ActionResult GetReportList(BaseParams baseParams)
        {
            var reportProvider = Container.Resolve<IGkhReportService>();

            return new JsonListResult(reportProvider.GetReportList(baseParams));
        }

        public ActionResult ReportPrint(BaseParams baseParams)
        {

            var file = Container.Resolve<IGkhReportService>().GetReport(baseParams);

            // Хак для отображения русских имен файлов
            file.FileDownloadName = WebUtility.UrlEncode(file.FileDownloadName).Replace("+", "%20");

            return file;
        }
    }
}