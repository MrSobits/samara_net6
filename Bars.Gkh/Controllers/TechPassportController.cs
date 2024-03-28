namespace Bars.Gkh.Controllers
{
    using System.IO;
    using System.Net;

    using Microsoft.AspNetCore.Mvc;
    using B4;

    using Bars.B4.Utils.Web;

    using DomainService;

    using Entities;

    public class TechPassportController : B4.Alt.DataController<TehPassport>
    {
        public ActionResult GetForm(BaseParams baseParams)
        {
            var result = (BaseDataResult)Container.Resolve<ITechPassportService>().GetForm(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult UpdateForm(BaseParams baseParams)
        {
            var result = (BaseDataResult)Container.Resolve<ITechPassportService>().UpdateForm(baseParams);
            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        }

        public ActionResult GetReportId(BaseParams baseParams)
        {
            var result = (BaseDataResult)Container.Resolve<ITechPassportService>().GetReportId(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult GetPrintFormResult(BaseParams baseParams)
        {
            var result = (BaseDataResult)Container.Resolve<ITechPassportService>().GetPrintFormResult(baseParams);

            string fileName;

            // Хак для отображения русских имен файлов
            if (UserAgentInfo.GetBrowserName(this.ControllerContext.HttpContext.Request).StartsWith("IE"))
            {
                fileName = WebUtility.UrlEncode(result.Message) ?? "report.xlsx";
                fileName = fileName.Replace("+", "%20");
            }
            else
            {
                fileName = result.Message.Replace("\"", "'");
            }

            return new FileStreamResult((MemoryStream)result.Data, "application/vnd.ms-excel") { FileDownloadName = fileName };
        }
    }
}
