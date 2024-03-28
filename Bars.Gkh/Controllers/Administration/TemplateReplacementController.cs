namespace Bars.Gkh.Controllers
{
    using System.IO;
    using System.Net;

    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.B4.Utils.Web;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;

    public class TemplateReplacementController : FileStorageDataController<TemplateReplacement>
    {
        public ITemplateReplacementService Service { get; set; }

        public ActionResult ListReports(BaseParams baseParams)
        {
            var result = (ListDataResult)Service.ListReports(baseParams);
            return new JsonNetResult(new { success = true, data = result.Data, totalCount = result.TotalCount });
        }

        public void GetBaseTemplate(BaseParams baseParams)
        {
            var result = Service.GetBaseTemplate(baseParams);
            if (result.Success)
            {
                var template = (Report.TemplateInfo)result.Data;

                var fileName = template.Name + "." + result.Message;

                // Хак для отображения русских имен файлов
                fileName = UserAgentInfo.GetBrowserName(ControllerContext.HttpContext.Request).StartsWith("IE")
                    ? WebUtility.UrlEncode(fileName).Replace("+", "%20")
                    : fileName.Replace("\"", "'");

                /*
                var mimeType = MimeTypeHelper.GetMimeType(Path.GetExtension(fileName));

                var stream = new MemoryStream(template.Template);

                return new FileStreamResult(stream, mimeType) { FileDownloadName = fileName };
                 */

                this.Response.ContentType = "application/octet-stream";
                this.Response.Headers.Add("Content-Disposition", "attachment; filename=\"" + fileName + "\"");
                this.Response.StatusCode = 200;
                this.Response.Body.Write(template.Template);

            }

            //return new JsonNetResult(new { success = false, message = "Не удалось сформирвоать файл" });
        }
    }
}