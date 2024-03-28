namespace Bars.Gkh.Overhaul
{
    using System.IO;

    using Bars.B4;
    using Bars.B4.Utils.Web;
    using Bars.Gkh.Overhaul.DomainService;

    using Microsoft.AspNetCore.Mvc;

    public class RealtyObjectDataController : BaseController
    {
        public ActionResult GetPrintFormResult(BaseParams baseParams)
        {
            var result = (BaseDataResult)this.Container.Resolve<IRealtyObjectDataService>().GetPrintFormResult(baseParams);

            string fileName;

            // Хак для отображения русских имен файлов
            if (UserAgentInfo.GetBrowserName(this.ControllerContext.HttpContext.Request).StartsWith("IE"))
            {
                fileName = System.Web.HttpUtility.UrlEncode(result.Message) ?? "report.xlsx";
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