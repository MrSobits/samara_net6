namespace Bars.GisIntegration.Base.File.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;

    using Bars.B4.Modules.FileStorage;

    using Castle.Windsor;

    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;

    /// <summary>
    /// Загрузчик файлов
    /// Реализует загрузку файла частями
    /// </summary>
    public class FileUploader : BaseFileUploader
    {
        //5 МБ = 5242880 Б 
        private const int FilePartSize = 5242880;

        /// <summary>
        /// Конструктор загрузчика файлов
        /// </summary>
        /// <param name="container">IoC Container</param>
        public FileUploader(IWindsorContainer container)
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
                var uploadId = this.InitializeSession(fileInfo, orgPpaGuid);

                this.UploadFileByParts(fileInfo, orgPpaGuid, uploadId);

                this.EndSession(orgPpaGuid, uploadId);

                return new FileUploadResult(uploadId);
            }
            catch (Exception exception)
            {
                return new FileUploadResult(exception);
            }
        }

        private string InitializeSession(FileInfo fileInfo, string orgPpaGuid)
        {
            var request = this.CreateInitializeSessionRequest(fileInfo, orgPpaGuid);

            var response = request.GetResponse();

            return response.Headers["X-Upload-UploadID"];
        }

        private WebRequest CreateInitializeSessionRequest(FileInfo fileInfo, string orgPpaGuid)
        {
            var requestUrl = $"{this.FileServiceProvider.ServiceAddress}/?upload";
            var result = this.CreateRequest(requestUrl);

            var fileSize = fileInfo.Size;
            var partCount = this.GetPartCount(fileInfo);
            
            result.Headers.Add("X-Upload-OrgPPAGUID", orgPpaGuid);
            result.Headers.Add("X-Upload-Filename", Uri.EscapeDataString(fileInfo.FullName));
            result.Headers.Add("X-Upload-Length", fileSize.ToString());
            result.Headers.Add("X-Upload-Part-Count", partCount.ToString());

            result.Method = "POST";

            return result;
        }

        private int GetPartCount(FileInfo fileInfo)
        {
            var fileSize = fileInfo.Size;
            var partCount = fileSize / FileUploader.FilePartSize;

            if (fileSize % FileUploader.FilePartSize > 0)
            {
                partCount++;
            }

            return (int)partCount;
        }

        private void EndSession(string orgPpaGuid, string uploadId)
        {
            var request = this.CreateEndSessionRequest(orgPpaGuid, uploadId);

            request.GetResponse();
        }

        private WebRequest CreateEndSessionRequest(string orgPpaGuid, string uploadId)
        {
            var requestUrl = $"{this.FileServiceProvider.ServiceAddress}/{uploadId}?completed";
            var result = this.CreateRequest(requestUrl);

            result.Headers.Add("X-Upload-OrgPPAGUID", orgPpaGuid);

            result.Method = "POST";

            return result;
        }

        private void UploadFileByParts(FileInfo fileInfo, string orgPpaGuid, string uploadId)
        {
            var partCount = this.GetPartCount(fileInfo);
            var fileData = this.GetFileData(fileInfo);

            var tasks = new List<Task>(partCount);
            for (var partNumber = 1; partNumber <= partCount; partNumber++)
            {
                tasks.Add(this.UploadPart(fileData, orgPpaGuid, uploadId, partNumber));
            }

            Task.WaitAll(tasks.ToArray());
        }

        private Task UploadPart(byte[] fileData, string orgPpaGuid, string uploadId, int partNumber)
        {
            return Task.Run(
                async () =>
                    {
                        var request = await this.CreateUploadPartRequest(fileData, orgPpaGuid, uploadId, partNumber);
                        await request.GetResponseAsync();
                    });
        }

        private async Task<WebRequest> CreateUploadPartRequest(byte[] fileData, string orgPpaGuid, string uploadId, int partNumber)
        {
            var requestUrl = $"{this.FileServiceProvider.ServiceAddress}/{uploadId}";
            var result = this.CreateRequest(requestUrl);

            var offset = (partNumber - 1) * FileUploader.FilePartSize;
            var partSize = Math.Min(fileData.Length - offset, FileUploader.FilePartSize);

            var hash = this.GetMd5Hash(fileData, offset, partSize);

            result.Headers.Add("X-Upload-OrgPPAGUID", orgPpaGuid);
            result.Headers.Add("X-Upload-Partnumber", partNumber.ToString());

            result.Headers.Add("Content-MD5", hash);

            result.Method = "PUT";

            result.ContentLength = partSize;

            using (var reqStream = result.GetRequestStream())
            {
                await reqStream.WriteAsync(fileData, offset, partSize);
            }

            return result;
        }

        private byte[] GetFileData(FileInfo fileInfo)
        {
            var fileManager = this.Container.Resolve<IFileManager>();

            try
            {
                using (var fileStream = fileManager.GetFile(fileInfo))
                {
                    var memoryStream = fileStream as MemoryStream;
                    if (memoryStream != null)
                    {
                        return memoryStream.ToArray();
                    }

                    using (var result = new MemoryStream((int)fileInfo.Size))
                    {
                        fileStream.Seek(0, SeekOrigin.Begin);
                        fileStream.CopyTo(result);
                        return result.ToArray();
                    }
                }
            }
            finally
            {
                this.Container.Release(fileManager);
            }
        }
    }
}