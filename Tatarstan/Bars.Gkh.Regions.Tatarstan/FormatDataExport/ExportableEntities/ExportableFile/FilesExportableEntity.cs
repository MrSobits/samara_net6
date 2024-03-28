namespace Bars.Gkh.Regions.Tatarstan.FormatDataExport.ExportableEntities.ExportableFile
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Config;
    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.ExportableEntities.ExportableFile;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Экспортируемые файлы
    /// </summary>
    public class FilesExportableEntity : BaseFilesExportableEntity
    {
        private readonly string storagePath;

        public FilesExportableEntity(IConfigProvider configProvider)
        {
            this.storagePath = configProvider?.GetConfig()
                .ModulesConfig["Bars.B4.Modules.FileSystemStorage"]
                .GetAs("FileDirectory", string.Empty) ?? string.Empty;
        }

        /// <inheritdoc />
        public override IDataResult<IList<List<string>>> GetData(DynamicDictionary baseParams)
        {
            var withoutAttachment = baseParams.GetAs("WithoutAttachment", false);

            if (withoutAttachment)
            {
                return new GenericDataResult<IList<List<string>>>();
            }

            var fileCollection = this.GetFiles();
            var data = fileCollection.Select(
                    x => new List<string>
                    {
                        x.Id.ToStr(),
                        x.FileName,
                        x.FullName.Cut(100),
                        x.Size.ToStr(),
                        x.Description.Cut(500),
                        x.FullPath
                    })
                .ToList();
            return new GenericDataResult<IList<List<string>>>(data);
        }

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код файла в системе отправителя",
                "Наименование файла",
                "Размер файла в байтах",
                "Описание файла",
                "Каталог файла",
                "Путь до файла в хранилище"
            };
        }

        /// <inheritdoc />
        public override ICollection<ExportFileStream> GetFileStreams()
        {
            return new ExportFileStream[0];
        }

        /// <inheritdoc />
        public override bool AddFile(ExportableFileInfo fileInfo)
        {
            fileInfo.FullPath = this.GetFilePath(fileInfo);

            if (!this.ValidateFile(fileInfo))
            {
                return false;
            }

            this.Files.Add(fileInfo);
            return true;
        }

        /// <inheritdoc />
        public override IEnumerable<ExportableFileInfo> AddFileRange(IEnumerable<ExportableFileInfo> fileInfoRange)
        {
            var validFiles = fileInfoRange.AsParallel()
                .Where(this.ValidateFile);

            foreach (var exportableFileInfo in validFiles)
            {
                this.Files.Add(exportableFileInfo);
            }

            return validFiles;
        }

        private bool ValidateFile(ExportableFileInfo fileInfo)
        {
            return this.AllowExtensionList.Contains(fileInfo.Extention.ToLowerInvariant()) &&
                File.Exists(fileInfo.FullPath);
        }

        private string GetFilePath(ExportableFileInfo fileInfo)
        {
            return Path.Combine(this.storagePath,
                fileInfo.ObjectCreateDate.Year.ToString(),
                fileInfo.ObjectCreateDate.Month.ToString(),
                $"{fileInfo.Id}.{fileInfo.Extention}");
        }
    }
}