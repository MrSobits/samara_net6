using Bars.B4.IoC;
using Bars.B4.Utils;

namespace Sobits.GisGkh.File.Impl
{
    using System;
    using System.IO;
    using System.Net;
    using Bars.B4.Modules.FileStorage;
    using Castle.Windsor;
    using GisGkhLibrary.Services;
    using GisGkhLibrary.Utils;
    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;

    /// <summary>
    /// Класс для скачивания файлов
    /// </summary>
    public class FileDownloader : BaseFileDownloader
    {
        FileServiceProvider provider;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="container">IoC Container</param>
        public FileDownloader(IWindsorContainer container)
            : base(container)
        {
            provider = new FileServiceProvider();
        }

        /// <summary>
        /// Получить файл с рест-сервиса
        /// </summary>
        /// <param name="fileGuid">Гуид файла</param>
        /// <param name="orgPpaGuid">Идентификатор зарегистрированной организации</param>
        /// <returns>Файл</returns>
        public override FileDownloadResult DownloadFile(string fileGuid, string orgPpaGuid)
        {
            try
            {
                var result = GetFileInfo(fileGuid, orgPpaGuid);
                var length = result.Headers["Content-Length"].ToInt();
                var fileName = Uri.UnescapeDataString(MimeUtility.Decode(result.Headers["X-Upload-Filename"]));

                var file = length >= (FilePartSize - 1) ? this.GetFileByParts(fileGuid, orgPpaGuid, length, fileName) : this.GetFile(fileGuid, orgPpaGuid, fileName);

                return new FileDownloadResult(file);
            }
            catch (Exception exception)
            {
                return new FileDownloadResult(exception);
            }
        }

        private WebResponse GetFileInfo(string guid, string orgPpaGuid)
        {
            var requestUrl = $"{provider.ServiceAddress}/{guid}";
            var result = (HttpWebRequest)WebRequest.Create(requestUrl);

            result.Headers.Add("X-Upload-OrgPPAGUID", orgPpaGuid);
            result.Method = "HEAD";
            return result.GetResponse();
        }

        private FileInfo GetFile(string guid, string orgPpaGuid, string fileName)
        {
            byte[] buffer = new byte[1024];
            var requestUrl = $"{provider.ServiceAddress}/{guid}?getfile";
            var result = (HttpWebRequest)WebRequest.Create(requestUrl);

            result.Headers.Add("X-Upload-OrgPPAGUID", orgPpaGuid);
            result.Timeout = 3000;
            result.Method = "GET";

            using (var myResponse = result.GetResponse())
            using (var stream = myResponse.GetResponseStream())
            using (var memoryStream = new MemoryStream())
            {
                int count;
                while (stream != null && (count = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    memoryStream.Write(buffer, 0, count);
                }

                memoryStream.Flush();
                return this.SaveFile(memoryStream, fileName);
            }
        }

        private FileInfo GetFileByParts(string guid, string orgPpaGuid, int fileSize, string fileName)
        {
            byte[] buffer = new byte[1024];
            var partCount = Math.Ceiling(fileSize.ToDecimal() / (FilePartSize - 1));

            using (var memoryStream = new MemoryStream())
            {
                for (var partNumber = 0; partNumber < partCount; partNumber++)
                {
                    var fromByte = partNumber * (FilePartSize - 1) + partNumber;
                    var toByte = partNumber == partCount - 1 ? fileSize - 1 : (partNumber + 1) * (FilePartSize - 1) + partNumber;

                    var requestUrl = $"{provider.ServiceAddress}/{guid}?getfile";

                    var result = (HttpWebRequest)WebRequest.Create(requestUrl);

                    result.Headers.Add("X-Upload-OrgPPAGUID", orgPpaGuid);
                    result.AddRange(fromByte, toByte);
                    result.Timeout = 3000;
                    result.Method = "GET";

                    using (var myResponse = result.GetResponse())
                    using (var stream = myResponse.GetResponseStream())
                    {
                        int count;
                        while (stream != null && (count = stream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            memoryStream.Write(buffer, 0, count);
                        }
                    }
                }

                memoryStream.Flush();
                return this.SaveFile(memoryStream, fileName);
            }
        }

        private FileInfo SaveFile(Stream fileStream, string fileName)
        {
            var fileManager = Container.Resolve<IFileManager>();
            return fileManager.SaveFile(fileStream, fileName);
            //using (Container.Using(fileManager))
            //{
            //    return fileManager.Import(fileStream, WebUtility.UrlDecode(fileName), false) as FileMetadata;
            //}
        }
    }
}