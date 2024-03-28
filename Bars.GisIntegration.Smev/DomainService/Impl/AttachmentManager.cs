namespace Bars.GisIntegration.Smev.DomainService.Impl
{
    using System.IO;

    using Bars.B4.Config;
    using Bars.B4.Modules.FileStorage;
    using Bars.GisIntegration.Smev.Entity;
    using Bars.Gkh.Utils;

    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;

    public class AttachmentManager : IAttachmentManager
    {
        /// <summary>
        /// Файловый менеджер
        /// </summary>
        public IFileManager FileManager { get; set; }
        
        /// <summary>
        /// Конфиг
        /// </summary>
        public IConfigProvider ConfigProvider { get; set; }

        /// <summary>
        /// Запаковать файлы в вложение
        /// </summary>
        /// <param name="files">Файлы</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <param name="fileName">Наименование файла</param>
        /// <param name="id">Идентифиактор для наименования архива</param>
        /// <returns>Вложение</returns>
        public FileMetadata PackFiles(FileInfo file, string id)
        {
            var stream = this.FileManager.GetFile(file);

            var fileMetadata = this.CreateEntity(file.FullName.Replace("\"", "'"), file.CheckSum, stream.Length);
            
            var cachedName = this.UploadFile($"{id.ToLower()}/{file.FullName}", stream);
            fileMetadata.CachedName = cachedName;

            return fileMetadata;
        }

        private string UploadFile(string name, Stream stream)
        {
            var appSettings = this.ConfigProvider.GetConfig().GetModuleConfig("Bars.GisIntegration.Smev");
            var ftpServer = appSettings.GetAs<string>("ftpServer");
            var ftpUser = appSettings.GetAs<string>("ftpUser");
            var ftpPassword = appSettings.GetAs<string>("ftpPassword");
            var ftpUtility = new FtpUtility(ftpServer, ftpUser, ftpPassword);

            return ftpUtility.UploadFile(stream, name, true);
        }
        
        private FileMetadata CreateEntity(string name, string checkSum, long size)
        {
            var fileName = Path.GetFileNameWithoutExtension(name);
            var extension = Path.GetExtension(name).TrimStart('.');
            var value = new FileMetadata
            {
                Checksum = checkSum,
                Extension = extension,
                Name = fileName,
                Size = size
            };

            return value;
        }
    }
}