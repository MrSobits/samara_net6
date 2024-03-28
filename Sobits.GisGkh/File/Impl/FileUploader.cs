namespace Sobits.GisGkh.File.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;
    using Bars.B4.Modules.FileStorage;
    using Castle.Windsor;
    using GisGkhLibrary.Enums;
    using GisGkhLibrary.Services;
    using GisGkhLibrary.Utils;

    /// <summary>
    /// Загрузчик файлов
    /// Реализует загрузку файла частями
    /// </summary>
    public class FileUploader : BaseFileUploader
    {
        FileServiceProvider provider;

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
        public override FileUploadResult UploadFile(GisFileRepository repository, Bars.B4.Modules.FileStorage.FileInfo fileInfo, string orgPpaGuid)
        {
            try
            {
                var uploadId = InitializeSession(repository, fileInfo, orgPpaGuid);

                UploadFileByParts(fileInfo, orgPpaGuid, uploadId);

                EndSession(orgPpaGuid, uploadId);

                return new FileUploadResult(uploadId);
            }
            catch (Exception exception)
            {
                return new FileUploadResult(exception);
            }
        }

        private string InitializeSession(GisFileRepository repository, Bars.B4.Modules.FileStorage.FileInfo fileInfo, string orgPpaGuid)
        {
            var request = CreateInitializeSessionRequest(repository, fileInfo, orgPpaGuid);

            var response = request.GetResponse();

            return response.Headers["X-Upload-UploadID"];
        }

        private WebRequest CreateInitializeSessionRequest(GisFileRepository repository, Bars.B4.Modules.FileStorage.FileInfo fileInfo, string orgPpaGuid)
        {
            //var fileManager = Container.Resolve<IFileManager>();
            //using (var metadata = fileManager.Export(fileInfo.Id))
            //{
            //var _fileManager = Container.Resolve<IFileManager>();
            //var fileStream = _fileManager.GetFile(fileInfo);
            //var file = fileStream.ReadAllBytes();
            //var fileName = fileInfo.FullName;

            //var serviceAddress = FileServiceProvider.ServiceAddress;
            //var result = CreateRequest(serviceAddress);
            provider = new FileServiceProvider();
            provider.FileStorageType = repository;
            //var result = CreateRequest(repository, FileServiceProvider.ServiceAddress);
            var result = (HttpWebRequest)WebRequest.Create(provider.ServiceAddress);

            var fileSize = fileInfo.Size;
            var partCount = GetPartCount(fileInfo);

            result.Headers.Add("X-Upload-OrgPPAGUID", orgPpaGuid);
            result.Headers.Add("X-Upload-Filename", MimeUtility.Encode(fileInfo.FullName));
            result.Headers.Add("X-Upload-Length", fileSize.ToString());
            result.Headers.Add("X-Upload-Part-Count", partCount.ToString());

            result.Method = "POST";

            return result;
        }

        private int GetPartCount(Bars.B4.Modules.FileStorage.FileInfo fileInfo)
        {
            var fileSize = fileInfo.Size;
            var partCount = fileSize / FilePartSize;

            if (fileSize % FilePartSize > 0)
            {
                partCount++;
            }

            return (int)partCount;
        }

        private void EndSession(string orgPpaGuid, string uploadId)
        {
            var request = CreateEndSessionRequest(orgPpaGuid, uploadId);

            request.GetResponse();
        }

        private WebRequest CreateEndSessionRequest(string orgPpaGuid, string uploadId)
        {
            var requestUrl = $"{provider.ServiceAddress}/{uploadId}?completed";
            var result = (HttpWebRequest)WebRequest.Create(requestUrl);

            result.Headers.Add("X-Upload-OrgPPAGUID", orgPpaGuid);

            result.Method = "POST";

            return result;
        }

        private void UploadFileByParts(Bars.B4.Modules.FileStorage.FileInfo fileInfo, string orgPpaGuid, string uploadId)
        {
            var partCount = GetPartCount(fileInfo);
            var fileData = GetFileData(fileInfo);

            var tasks = new List<Task>(partCount);
            for (var partNumber = 1; partNumber <= partCount; partNumber++)
            {
                tasks.Add(UploadPart(fileData, orgPpaGuid, uploadId, partNumber));
            }

            Task.WaitAll(tasks.ToArray());
        }

        private Task UploadPart(byte[] fileData, string orgPpaGuid, string uploadId, int partNumber)
        {
            return Task.Run(
                async () =>
                {
                    var request = await CreateUploadPartRequest(fileData, orgPpaGuid, uploadId, partNumber);
                    await request.GetResponseAsync();
                });
        }

        private async Task<WebRequest> CreateUploadPartRequest(byte[] fileData, string orgPpaGuid, string uploadId, int partNumber)
        {
            var requestUrl = $"{provider.ServiceAddress}/{uploadId}";
            var result = (HttpWebRequest)WebRequest.Create(requestUrl);

            var offset = (partNumber - 1) * FilePartSize;
            var partSize = Math.Min(fileData.Length - offset, FilePartSize);

            var hash = GetMd5Hash(fileData, offset, partSize);

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

        private byte[] GetFileData(Bars.B4.Modules.FileStorage.FileInfo fileInfo)
        {
            var fileManager = Container.Resolve<IFileManager>();

            try
            {
                //var fileStream = fileManager.GetFile(fileInfo);
                //var file = fileStream.ReadAllBytes();


                //var metadata = fileManager.Export(fileInfo.Id);
                //fileInfo.FileName = metadata.FileName;
                //fileInfo.CachedName = metadata.CachedName;
                //fileInfo.CheckSum = metadata.CheckSum;
                //fileInfo.Size = metadata.Size;
                //fileInfo.LastAccess = metadata.LastAccess;

                using (var fileStream = fileManager.GetFile(fileInfo))
                {
                    if (fileStream is MemoryStream memoryStream)
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
                Container.Release(fileManager);
            }
        }
    }
}