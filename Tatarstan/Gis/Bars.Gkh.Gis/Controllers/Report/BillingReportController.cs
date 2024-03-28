namespace Bars.Gkh.Gis.Controllers.Report
{
    using System.Globalization;
    using System.IO;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using B4.Modules.FileStorage;
    using B4.Utils.Web;

    using Bars.B4.Modules.Analytics.Reports.Entities;

    using DomainService.Report;

    public class BillingReportController : B4.Alt.DataController<PrintForm>
    {
        public IFileManager FileManager { get; set; }
        public IBillingReportService BillingReportService { get; set; }


        /// <summary>
        /// Получение отчета
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult GetReport(BaseParams baseParams)
        {
            var result = BillingReportService.GetReport(baseParams);

            //Заглушка
            //var text = System.Text.Encoding.UTF8.GetBytes("Hello world");
            //(result.Data as MemoryStream).Write(text, 0, text.Length);

            //Если имя сохраняемого файла отсутствует
            if (string.IsNullOrEmpty(result.Message))
            {
                //конвертируем отчет в текст
                var ms = result.Data as MemoryStream;
                if (ms != null)
                {
                    ms.Position = 0;
                    var sr = new StreamReader(ms);
                    var myStr = sr.ReadToEnd();

                    return Json(myStr);
                }
            }

            //сохраняем файл отчета
            var fileId = Path.Combine(HttpContext.Request.PathBase.Value ?? "",
                                      @"fileupload/download?id=" +
                                      FileManager.SaveFile((MemoryStream)result.Data, result.Message).Id);
            return new JsonNetResult(fileId);
        }

        /// <summary>
        /// Получение списка форматов, в которых можно получить отчет
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult GetExportFormatList(BaseParams baseParams)
        {
            var result = BillingReportService.GetExportFormatList(baseParams);
            return new JsonNetResult(new
            {
                formatList = result.Data
            });
        }
    }



    public class InlineFileResult : ActionResult
    {
        public InlineFileResult(byte[] data, string fileName)
        {
            Data = data;
            FileName = fileName;
        }

        public byte[] Data { get; set; }

        public string FileName { get; set; }

        public ResultCode ResultCode { get; set; }

        public override void ExecuteResult(ActionContext context)
        {
            switch (ResultCode)
            {
                case ResultCode.Success:
                    var extention = Path.GetExtension(FileName) ?? string.Empty;
                    switch (extention)
                    {
                        case ".htm":
                        case ".html":
                            context.HttpContext.Response.ContentType = "text/html; charset=utf-8";
                            break;
                        case ".bmp":
                            context.HttpContext.Response.ContentType = "Image/bmp";
                            break;
                        case ".gif":
                            context.HttpContext.Response.ContentType = "Image/gif";
                            break;
                        case ".jpg":
                            context.HttpContext.Response.ContentType = "Image/jpeg";
                            break;
                        case ".png":
                            context.HttpContext.Response.ContentType = "Image/png";
                            break;
                        case ".pdf":
                            context.HttpContext.Response.ContentType = "application/pdf";
                            break;
                        //case ".zip":
                        //    context.HttpContext.Response.ContentType = "application/zip";
                        //    break;
                        default:
                            context.HttpContext.Response.ContentType = "application/octet-stream";
                            break;
                    }

                    
                    context.HttpContext.Response.Headers.Add("Content-Disposition", "inline; filename=" + FileName);
                    context.HttpContext.Response.Headers.Add("Content-Length",
                                                           Data.Length.ToString(CultureInfo.InvariantCulture));
                    context.HttpContext.Response.Body.WriteAsync(Data);

                    break;

                case ResultCode.FileNotFound:
                    context.HttpContext.Response.StatusCode = 404;
                    // TODO: Заменить на аналог из Net Core
                    //context.HttpContext.Response.Body.Write("File not found");
                    break;
            }
        }
    }

}
