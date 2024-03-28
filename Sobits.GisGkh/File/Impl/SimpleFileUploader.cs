using Bars.B4.IoC;

namespace Sobits.GisGkh.File.Impl
{
    using System;
    using System.Net;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Castle.Windsor;
    using GisGkhLibrary.Enums;
    using GisGkhLibrary.Services;
    using GisGkhLibrary.Utils;

    /// <summary>
    /// Загрузчик файлов
    /// Реализует простую загрузку файла в 1 запрос
    /// </summary>
    public class SimpleFileUploader : BaseFileUploader
    {
        FileServiceProvider provider;

        /// <summary>
        /// Конструктор загрузчика файлов
        /// </summary>
        /// <param name="container">IoC Container</param>
        public SimpleFileUploader(IWindsorContainer container)
            : base(container)
        {

        }

        /// <summary>
        /// Отправить файл на рест-сервис
        /// </summary>
        /// <param name="fileInfo">Файл</param>
        /// <param name="orgPpaGuid">Идентификатор зарегистрированной организации</param>
        /// <returns>Результат загрузки файла</returns>
        public override FileUploadResult UploadFile(GisFileRepository repository, FileInfo fileInfo, string orgPpaGuid)
        {
            try
            {
                var webRequest = this.CreateUploadFileRequest(repository, fileInfo, orgPpaGuid);
                var response = webRequest.GetResponse();

                return new FileUploadResult(response.Headers["X-Upload-UploadID"]);
            }
            catch (Exception ex)
            {
                return new FileUploadResult(ex);
            }
        }

        private WebRequest CreateUploadFileRequest(GisFileRepository repository, FileInfo fileInfo, string orgPpaGuid)
        {
            //IRisFileManager fileManager;
            //using (var metadata = fileManager.Export(fileInfo.Id))
            //{
            var _fileManager = Container.Resolve<IFileManager>();
            var fileStream = _fileManager.GetFile(fileInfo);
            var file = fileStream.ReadAllBytes();
            var fileName = fileInfo.FullName;
            provider = new FileServiceProvider();
            provider.FileStorageType = repository;
                //var result = CreateRequest(repository, FileServiceProvider.ServiceAddress);
            var result = (HttpWebRequest)WebRequest.Create(provider.ServiceAddress);
            //var request = new HttpRequestMessage(httpMethod, GetGisUri(repository))



                //fileInfo.Size = metadata.Size;

                result.Headers.Add("Content-MD5", GetMd5Hash(file));
                result.Headers.Add("X-Upload-OrgPPAGUID", orgPpaGuid);
                result.Headers.Add("X-Upload-Filename", MimeUtility.Encode(fileInfo.FullName));
                result.Method = "PUT";

                using (var reqStream = result.GetRequestStream())
                {
                    fileStream.CopyTo(reqStream);
                    return result;
                }
            //}
        }
    }
}