namespace Bars.Gkh.SystemDataTransfer.Meta
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Помощник импорта файлов
    /// </summary>
    public class FileProcessingHelper : IDisposable
    {
        private readonly string tempPath;
        private readonly string templFolderName;
        private readonly IDictionary<string, string> files;

        /// <summary>
        /// .ctor
        /// </summary>
        public FileProcessingHelper()
        {
            this.tempPath = Path.GetTempPath();
            this.templFolderName = Path.GetRandomFileName();
            this.files = new Dictionary<string, string>();

            var path = Path.Combine(this.tempPath, this.templFolderName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// Метод добавляет файл в папку TEMP
        /// </summary>
        /// <param name="id">Идентификатор файла</param>
        /// <param name="file">Поток</param>
        public void AddFile(string id, Stream file)
        {
            var filePath = this.GetFilePath(id);

            using (var fs = File.Create(filePath))
            {
                file.CopyTo(fs);
                this.files.Add(id, filePath);
            }
        }

        /// <summary>
        /// Вернуть файл
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Массив данныз</returns>
        public byte[] GetFile(string id)
        {
            var filePath = this.GetFilePath(id);

            using (var ms = new MemoryStream())
            using (var fs = File.Open(filePath, FileMode.Open))
            {
                fs.CopyTo(ms);
                ms.Position = 0;

                return ms.ToArray();
            }
        }

        private string GetFilePath(string id)
        {
            return Path.Combine(this.GetTempFolder(), id);
        }

        private string GetTempFolder()
        {
            return Path.Combine(this.tempPath, this.templFolderName);
        }

        /// <summary>
        /// Метод освобождения ресурсов (удаляет временную папку)
        /// </summary>
        public void Dispose()
        {
            if (Directory.Exists(this.GetTempFolder()))
            {
                Directory.Delete(this.GetTempFolder(), true);
            }
        }
    }
}