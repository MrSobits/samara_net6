namespace Bars.Gkh.Utils
{
    using Bars.B4.Utils.Web;
    using System;
    using System.Globalization;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Тип ответа превью файла
    /// </summary>
    public class DownloadPreviewResult : DownloadResult
    {
        /// <summary>
        /// Поток файла
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// Обработка результата
        /// </summary>
        /// <param name="context"></param>
        public override void ExecuteResult(ActionContext context)
        {
            switch (ResultCode)
            {
                case ResultCode.Success:
                    string normalizedFileName = GetNormalizedFileName(context);
                    var extenstion = System.IO.Path.GetExtension(normalizedFileName) ?? string.Empty;
                    bool formatSupported;
                    string fromFileExtension = GetContentTypeFromFileExtension(extenstion, out formatSupported);

                    context.HttpContext.Response.StatusCode = 200;
                    context.HttpContext.Response.Headers.Add("Content-Disposition", "inline; filename=\"" + normalizedFileName + "\"");
                    context.HttpContext.Response.ContentType = fromFileExtension;

                    //Если формат не поддерживается
                    if (!formatSupported)
                    {
                        // TODO: Найти замену
                        //context.HttpContext.Response.Write("<div style=\"top: 50 %; left: 50 %; position: fixed; margin - top: -100px; margin - left: -200px; \">Данный формат не поддерживается предпросмотром</div>");
                        return;
                    }

                    //Если передан массив байтов, то пишем его в ответ
                    if (Data != null)
                    {
                        context.HttpContext.Response.Headers.Add("Content-Length", Data.Length.ToString(CultureInfo.InvariantCulture));
                        // TODO: Найти замену
                        //context.HttpContext.Response.BinaryWrite(Data);
                    }
                    else
                    {
                        //Иначе просто указываем путь к файлу
                        WriteFileContents(context);
                    }

                    break;
                case ResultCode.FileNotFound:
                    context.HttpContext.Response.StatusCode = 404;
                    // TODO: Найти замену
                    //context.HttpContext.Response.Write("File not found");
                    break;
            }
        }

        /// <summary>
        /// Получение типа файла по расширению
        /// </summary>
        protected string GetContentTypeFromFileExtension(string fileExtension, out bool formatSupported)
        {
            formatSupported = true;
            string str;
            switch (fileExtension)
            {
                case ".bmp":
                case ".xls":
                case ".xlsx":
                    str = "Image/bmp";
                    break;
                case ".gif":
                    str = "Image/gif";
                    break;
                case ".jpg":
                case ".jpeg":
                    str = "Image/jpeg";
                    break;
                case ".png":
                    str = "Image/png";
                    break;
                case ".tif":
                    str = "Image/tiff";
                    break;
                case ".pdf":
                    str = "application/pdf";
                    break;
                case ".txt":
                    str = "text/plain";
                    break;
                default:
                    str = "text/html";
                    formatSupported = false;
                    break;
            }
            return str;
        }
    }
}
