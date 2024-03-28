namespace Bars.Gkh.FileManager
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Web;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.Config;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.FileSystemStorage;
    using Bars.B4.Utils.Web;

    using Castle.Windsor;

    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;

    /// <summary>
    /// Gkh реализация файл-менеджера для хранения файлов в файловой системе
    /// Заменяем полностью, так как часть членов скрыта
    /// </summary>
    public class FileSystemFileManager : IFileManagerSystem
    {
        private DirectoryInfo filesDirectory;

        /// <summary>
        /// Домен-сервис для <see cref="FileInfoForDelete"/>
        /// </summary>
        public IDomainService<FileInfoForDelete> FileInfoForDeleteDomainService { get; set; }

        /// <summary>
        /// Домен-сервис для <see cref="FileInfo"/>
        /// </summary>
        public IDomainService<FileInfo> FileInfoDomainService { get; set; }

        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Директория хранения файлов
        /// </summary>
        public DirectoryInfo FilesDirectory
        {
            get
            {
                if (this.filesDirectory != null)
                {
                    return this.filesDirectory;
                }

                if (this.filesDirectory == null)
                {
                    var path = this.Container.Resolve<IConfigProvider>().GetConfig().ModulesConfig["Bars.B4.Modules.FileSystemStorage"].GetAs("FileDirectory", string.Empty);

                    this.filesDirectory =
                         new DirectoryInfo(
                             Path.IsPathRooted(path) ? path : ApplicationContext.Current.MapPath("~/" + path.TrimStart('~', '/')));
                }

                if (!this.filesDirectory.Exists)
                {
                    this.filesDirectory.Create();
                }

                return this.filesDirectory;
            }
        }

        /// <summary>
        /// Сохранить файл
        /// </summary>
        public virtual FileInfo SaveFile(string fileName, byte[] data)
        {
            var name = Path.GetFileNameWithoutExtension(fileName);
            var extension = Path.GetExtension(fileName).TrimStart('.');
            return this.SaveFile(name, extension, data);
        }

        /// <summary>
        /// Сохранить файл
        /// </summary>
        public virtual FileInfo SaveFile(Stream fileStream, string fileName)
        {
            fileStream.Seek(0, SeekOrigin.Begin);

            var fileInfo = this.CreateEntity(fileName.Replace("\"", "'"), this.ComputeChekSum(fileStream), fileStream.Length);
            var filePath = this.GetFilePath(fileInfo);
            var directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            fileStream.Seek(0, SeekOrigin.Begin);

            using (var outputStream = File.OpenWrite(filePath))
            {
                fileStream.CopyTo(outputStream);
                outputStream.Flush();
            }

            fileStream.Seek(0, SeekOrigin.Begin);

            return fileInfo;
        }

        /// <summary>
        /// Сохранить файл
        /// </summary>
        public virtual FileInfo SaveFile(FileData fileData)
        {
            return this.SaveFile(fileData.FileName, fileData.Extention, fileData.Data);
        }

        /// <summary>
        /// Сохранить файл
        /// </summary>
        public virtual FileInfo SaveFile(string name, string extension, byte[] data)
        {
            if (string.IsNullOrWhiteSpace(extension))
            {
                extension = Path.GetExtension(name);
                if (string.IsNullOrWhiteSpace(extension))
                {
                    throw new ValidationException("Не удалось определить расширение файла");
                }

                name = Path.GetFileNameWithoutExtension(name);
                extension = extension.TrimStart('.');
            }

            var fileInfo = this.CreateEntity(name, extension, this.ComputeChekSum(data), data.Length);
            var filePath = this.GetFilePath(fileInfo);
            var directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            File.WriteAllBytes(filePath, data);
            return fileInfo;
        }

        /// <summary>
        /// Загрузить файл
        /// </summary>
        public virtual DownloadResult LoadFile(object id)
        {
            var entity = this.FileInfoDomainService.Get(id);
            if (entity == null)
            {
                return new DownloadResult { ResultCode = ResultCode.FileNotFound };
            }

            var fileInfo = new System.IO.FileInfo(this.GetFilePath(entity));
            if (!fileInfo.Exists)
            {
                throw new FileNotFoundException();
            }

            return new DownloadResult
                {
                    ResultCode = ResultCode.Success,
                    FileDownloadName = HttpUtility.UrlPathEncode($"{entity.Name}.{entity.Extention}"),
                    Path = fileInfo.FullName
                };
        }

        /// <summary>
        /// Проверить файл
        /// </summary>
        public virtual BaseDataResult CheckFile(object id)
        {
            var entity = this.FileInfoDomainService.Get(id);
            if (entity == null)
            {
                return new BaseDataResult { Success = false };
            }

            var fileInfo = new System.IO.FileInfo(this.GetFilePath(entity));
            if (!fileInfo.Exists)
            {
                return new BaseDataResult { Success = false };
            }

            return new BaseDataResult { Success = true };
        }

        /// <summary>
        /// Получить файл
        /// </summary>
        public virtual Stream GetFile(FileInfo fileInfo)
        {
            var path = this.GetFilePath(fileInfo);
            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }

            return new MemoryStream(File.ReadAllBytes(path));
        }

        /// <summary>
        /// Получить в формате base-64
        /// </summary>
        public virtual string GetBase64String(FileInfo fileInfo)
        {
            string base64String;
            using (var stream = this.GetFile(fileInfo))
            {
                var buffer = new byte[stream.Length];

                stream.Read(buffer, 0, (int)stream.Length);
                base64String = Convert.ToBase64String(buffer);
            }

            return base64String;
        }

        ///// <summary>
        ///// Получить в формате UTF-8
        ///// </summary>
        //public virtual string GetUTF8String(FileInfo fileInfo)
        //{
        //    string base64String;
        //    using (var stream = this.GetFile(fileInfo))
        //    {
        //        var buffer = new byte[stream.Length];

        //        stream.Read(buffer, 0, (int)stream.Length);
        //        base64String = System.Text.Encoding.UTF8.GetString(buffer);
        //    }

        //    return base64String;
        //}

        /// <summary>
        /// Удалить файл
        /// </summary>
        public IDataResult DeleteFile(object id)
        {
            var entity = this.FileInfoForDeleteDomainService.GetAll().FirstOrDefault(x => x.FileInfoId == (long)id);
            if (entity != null)
            {
                var fileInfo = new System.IO.FileInfo(this.GetFilePath(entity));
                if (!fileInfo.Exists)
                {
                    return new FileStorageDataResult { Success = false, Message = "File not found" };
                }

                File.Delete(fileInfo.FullName);

                return new FileStorageDataResult { Success = true, Message = "File deleted" };
            }

            return new FileStorageDataResult { Success = false, Message = "File not found" };
        }

        private HashAlgorithm GetHasAlgorithm()
        {
            return MD5.Create();
        }

        private string ComputeChekSum(byte[] data)
        {          
            return Convert.ToBase64String(this.GetHasAlgorithm().ComputeHash(data));
        }

        private string ComputeChekSum(Stream stream)
        {
            return Convert.ToBase64String(this.GetHasAlgorithm().ComputeHash(stream));
        }

        protected virtual FileInfo CreateEntity(string name, string extension, string checkSum, long size)
        {
            var value = new FileInfo
            {
                CheckSum = checkSum,
                Extention = extension,
                Name = name,
                Size = size
            };

            this.FileInfoDomainService.Save(value);

            return value;
        }

        private FileInfo CreateEntity(string name, string checkSum, long size)
        {
            var fileName = Path.GetFileNameWithoutExtension(name);
            var extension = Path.GetExtension(name).TrimStart('.');
            return this.CreateEntity(fileName, extension, checkSum, size);
        }

        /// <summary>
        /// Получить полный путь до файла
        /// </summary>
        public string GetFilePath(FileInfo fileInfo)
        {
            return this.GetFilePath(fileInfo.Id, fileInfo.Extention, fileInfo.ObjectCreateDate);
        }

        private string GetFilePath(FileInfoForDelete fileInfoForDelete)
        {
            return this.GetFilePath(fileInfoForDelete.Id, fileInfoForDelete.Extention, fileInfoForDelete.ObjectCreateDate);
        }

        private string GetFilePath(long fileInfoId, string extension, DateTime objectCreateDate)
        {
            return Path.Combine(this.FilesDirectory.FullName, objectCreateDate.Year.ToString(), objectCreateDate.Month.ToString(), string.Format("{0}.{1}", fileInfoId, extension));
        }

        /// <inheritdoc />
        public void Delete(FileInfo fileInfo)
        {
            this.FileInfoDomainService.Delete(fileInfo.Id);
        }
    }
}