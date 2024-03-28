namespace Bars.GisIntegration.Base.File.Impl
{
    using System;
    using System.Net;

    using Bars.B4.Modules.FileStorage;

    using Castle.Windsor;

    /// <summary>
    /// Загрузчик файлов
    /// Реализует простую загрузку файла в 1 запрос
    /// </summary>
    public class SimpleFileUploader: BaseFileUploader
    {
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
        public override FileUploadResult UploadFile(FileInfo fileInfo, string orgPpaGuid)
        {
            try
            {
                var webRequest = this.CreateUploadFileRequest(fileInfo, orgPpaGuid);

                var response = webRequest.GetResponse();

                return new FileUploadResult(response.Headers["X-Upload-UploadID"]);
            }
            catch (Exception exception)
            {
                return new FileUploadResult(exception);
            }
        }

        private WebRequest CreateUploadFileRequest(FileInfo fileInfo, string orgPpaGuid)
        {
            var fileManager = this.Container.Resolve<IFileManager>();

            try
            {
                var serviceAddress = this.FileServiceProvider.ServiceAddress;
                var result = this.CreateRequest(serviceAddress);

                result.Headers.Add("Content-MD5", this.GetMd5Hash(fileInfo));
                result.Headers.Add("X-Upload-OrgPPAGUID", orgPpaGuid);
                result.Headers.Add("X-Upload-Filename", Uri.EscapeDataString(fileInfo.FullName));
                result.Method = "PUT";

                using (var reqStream = result.GetRequestStream())
                {
                    using (var fileStream = fileManager.GetFile(fileInfo))
                    {
                        fileStream.CopyTo(reqStream);
                    }
                }

                return result;
            }
            finally
            {
                this.Container.Release(fileManager);
            }
        }
    }
}
