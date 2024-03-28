using Bars.B4.Modules.FileStorage;
using GisGkhLibrary.Enums;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Castle.Windsor;
using Bars.B4.Utils;
using GisGkhLibrary.Utils;
using System.IO;

namespace GisGkhLibrary.Services
{
    public class FileExchange
    {

        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }
        // TODO: Найти замену
      /*  private WebRequestHandler RequestHandler { get; set; }

        public FileExchange()
        {

            RequestHandler = new WebRequestHandler
            {
                Proxy = new WebProxy("http://localhost:8090"),
                ClientCertificateOptions = ClientCertificateOption.Manual
            };
        }*/

        //public string UploadFile(GisFileRepository repository, byte[] file, string fileName, string OrgPPAGUID)
        //{
        //    using (var gisWebClient = new GisWebClient(RequestHandler))
        //    {
        //        return gisWebClient.UploadFile(repository, HttpMethod.Put, file, fileName, OrgPPAGUID);
        //    }
        //}

        public string UploadFile(GisFileRepository repository, Bars.B4.Modules.FileStorage.FileInfo fileInfo, string OrgPPAGUID, IWindsorContainer container)
        {
            return "";
            /* using (var gisWebClient = new GisWebClient(RequestHandler, container))
             {
                 return gisWebClient.UploadFile(repository, HttpMethod.Put, fileInfo, OrgPPAGUID);
             }*/
        }

        ///// <summary>
        ///// Проверка расширения файла на допустимое значение
        ///// </summary>
        ///// <param name="ext">расширения файла</param>
        ///// <returns>true - всё нормально</returns>
        //private bool CheckExtension(string ext)
        //{
        //    ext = ext.Trim(new char[] { ' ', '.' });

        //    string[] allowExt = new string[] { "pdf", "docx", "doc", "rtf", "txt", "xls", "xlsx", "jpeg", "jpg", "bmp",
        //        "tif", "tiff", "gif", "zip", "rar", "csv", "odp", "odf", "ods", "odt", "sxc", "sxw" };

        //    return allowExt.Contains(ext);
        //}
    }

    class GisWebClient : HttpClient
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }
        private HttpRequestHeaders RequestHeaders { get; set; }
        private HttpContentHeaders ContentHeaders { get; set; }

        private string Today { get; set; }

        /*public GisWebClient(WebRequestHandler webRequestHandler, IWindsorContainer container) : base(webRequestHandler)
        {
            this.Container = container;

            var cultureInfo = new CultureInfo("en-US");
            Today = DateTime.Now.ToString("ddd,' 'dd' 'MMM' 'yyyy' 'HH':'mm':'ss' 'K", cultureInfo);
        }*/

        //public string UploadFile(GisFileRepository repository, HttpMethod httpMethod, byte[] file, string fileName, string OrgPPAGUID)
        public string UploadFile(GisFileRepository repository, HttpMethod httpMethod, Bars.B4.Modules.FileStorage.FileInfo fileInfo, string OrgPPAGUID)
        {
            var _fileManager = Container.Resolve<IFileManager>();
            var fileStream = _fileManager.GetFile(fileInfo);
            var file = fileStream.ReadAllBytes();
            var fileName = fileInfo.FullName;
            var fileService = new FileExchange();

            var md5Hash = GetMd5Hash(file);

            using (var content = new ByteArrayContent(file))
            {

                content.Headers.ContentLength = file.LongLength;
                content.Headers.Add("Content-MD5", md5Hash);

                using (var request = new HttpRequestMessage(httpMethod, GetGisUri(repository)))
                {
                    request.Headers.Add("X-Upload-OrgPPAGUID", OrgPPAGUID);
                    request.Headers.Add("X-Upload-Filename", MimeUtility.Encode(fileInfo.FullName));
                    request.Content = content;

                    var response = SendAsync(request).ContinueWith(sendTask => {
                        return new UploadFileResult(sendTask);
                    });

                    response.Wait();

                    return response.Result.UploadId;
                }
            }
        }

        private Uri GetGisUri(GisFileRepository repository)
        {

            var host = $"127.0.0.1:8090/ext-bus-file-store-service/rest/{repository.ToString().Replace("_"," - ")}";
            var uriBuilder = new UriBuilder(host)
            {
                Scheme = "http"
            };

            return uriBuilder.Uri;
        }

        //static string CalculateMD5(byte[] file)
        //{
        //    using (var md5 = MD5.Create())
        //    {
        //        var hash = md5.ComputeHash(file);
        //        return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        //    }
        //}

        /// <summary>
        /// Получить хэш файла по алгоритму МД5
        /// </summary>
        /// <param name="stream">Поток данных</param>
        /// <returns>Хэш файла</returns>
        protected string GetMd5Hash(Stream stream)
        {
            if (stream.Position != 0) stream.Seek(0, SeekOrigin.Begin);

            try
            {
                using (var md5 = System.Security.Cryptography.MD5.Create())
                {
                    return Convert.ToBase64String(md5.ComputeHash(stream));
                }
            }
            finally
            {
                stream.Seek(0, SeekOrigin.Begin);
                stream.Flush();
            }
        }

        protected string GetMd5Hash(byte[] inputArray)
        {
            byte[] hash;

            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                hash = md5.ComputeHash(inputArray);
            }

            return Convert.ToBase64String(hash);
        }

        protected string GetMd5Hash(byte[] inputArray, int offset, int count)
        {
            byte[] hash;

            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                hash = md5.ComputeHash(inputArray, offset, count);
            }

            return Convert.ToBase64String(hash);
        }

        private string mimeDecode(string s)
        {
            string result = string.Empty;
            System.Text.RegularExpressions.MatchCollection matchList = System.Text.RegularExpressions.Regex.Matches(s, @"(?:=\?)([^\?]+)(?:\?B\?)([^\?]*)(?:\?=)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            for (int i = 0; i < matchList.Count; i++)
            {
                string charset = matchList[i].Groups[1].Value;
                string data = matchList[i].Groups[2].Value;
                byte[] b = Convert.FromBase64String(data);
                result += Encoding.GetEncoding(charset).GetString(b);
            }

            return (string.IsNullOrEmpty(result)) ? s : result;
        }

        public void DownloadFile()
        {
        }
    }

    public class UploadFileResult
    {
        public UploadFileResult(Task<HttpResponseMessage> uploadTask)
        {

            if (uploadTask.IsCanceled)
            {
                UploadResult = "Request was canceled";
                return;
            }

            if (uploadTask.IsFaulted)
            {
                UploadResult = "Request failed " + uploadTask.Exception;
                return;
            }

            var response = (HttpResponseMessage)uploadTask.Result;

            UploadResult = "Request complete with status code " + response.StatusCode;

            var uploadErrorHeader = response.Headers.FirstOrDefault(h => h.Key.Equals("X-Upload-Error"));
            if (uploadErrorHeader.Value != null && uploadErrorHeader.Value.Any())
            {
                UploadResult = uploadErrorHeader.Value.First();
                return;
            }

            var uploadIdHeader = response.Headers.FirstOrDefault(h => h.Key.Equals("X-Upload-UploadID"));
            UploadId = uploadIdHeader.Value.First();
        }

        public string UploadResult { get; set; }
        public string UploadId { get; set; }
    }
}
