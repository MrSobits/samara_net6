namespace Bars.Gkh.FormatDataExport.ExportableEntities.ExportableFile
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.Utils;

    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Базовый класс экспортируемых файлов
    /// </summary>
    public abstract class BaseFilesExportableEntity : IExportableEntity, IFileEntityCollection, IDisposable
    {
        /// <inheritdoc />
        public string Code => "FILES";

        protected ISet<ExportFileStream> Filestreams = new HashSet<ExportFileStream>();

        /// <summary>
        /// Логгер
        /// </summary>
        public ILogger LogManager { get; set; }

        /// <summary>
        /// Менеджер файлов
        /// </summary>
        public IFileManager FileManager { get; set; }

        /// <summary>
        /// Коллекция файлов
        /// </summary>
        protected readonly ConcurrentBag<ExportableFileInfo> Files = new ConcurrentBag<ExportableFileInfo>();

        /// <inheritdoc />
        public virtual IList<string> AllowExtensionList { get; } = new List<string>
        {
            "pdf", "docx", "doc", "rtf", "txt", "xls", "xlsx", "jpeg", "jpg",
            "png", "bmp", "tif", "tiff", "gif", "zip", "rar", "csv", "odp",
            "odf", "ods", "odt", "sxc", "sxw"
        };

        /// <inheritdoc />
        public abstract IDataResult<IList<List<string>>> GetData(DynamicDictionary baseParams);

        /// <inheritdoc />
        public abstract IList<string> GetHeader();

        /// <inheritdoc />
        public abstract ICollection<ExportFileStream> GetFileStreams();

        /// <inheritdoc />
        public virtual bool AddFile(ExportableFileInfo fileInfo)
        {
            if (!this.ValidateFile(fileInfo))
            {
                return false;
            }

            this.Files.Add(fileInfo);
            return true;
        }

        /// <inheritdoc />
        public virtual int Count => this.Files.Count;

        /// <inheritdoc />
        public virtual IEnumerable<ExportableFileInfo> AddFileRange(IEnumerable<ExportableFileInfo> fileRange)
        {
            var validFiles = fileRange.AsParallel()
                .Where(this.ValidateFile)
                .ToList();

            foreach (var file in validFiles)
            {
                this.Files.Add(file);
            }

            return validFiles;
        }

        /// <inheritdoc />
        public IEnumerable<ExportableFileInfo> GetFiles()
        {
            return this.Files.ToArray().DistinctBy(x => x.Id);
        }

        private bool ValidateFile(ExportableFileInfo fileInfo)
        {
            return this.AllowExtensionList.Contains(fileInfo.Extention.ToLowerInvariant()) &&
                this.FileManager.IsExistsFile(fileInfo);
        }

        /// <inheritdoc />
        public virtual string FormatVersion => BaseExportableEntity.Version;

        /// <inheritdoc />
        public IDictionary<long, IEnumerable<int>> EmptyMandatoryFields
        {
            get
            {
                var result = new Dictionary<long, IEnumerable<int>>();
                var data = this.GetData(null).Data;
                foreach (var row in data)
                {
                    var id = row.First().ToLong();
                    var emptyRows = row.Where(x => x.IsEmpty()).Select((s, i) => i);

                    if (emptyRows.IsNotEmpty())
                    {
                        result.Add(id, emptyRows);
                    }
                }

                return result;
            }
        }

        /// <inheritdoc />
        public IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>();
        }

        /// <inheritdoc />
        public virtual FormatDataExportProviderFlags AllowProviderFlags => FormatDataExportProviderFlags.All;

        protected void CleanFileStreams()
        {
            if (this.Filestreams != null)
            {
                foreach (var filestream in this.Filestreams)
                {
                    filestream?.Dispose();
                }

                this.Filestreams.Clear();
            }
        }

        public void Dispose()
        {
            this.CleanFileStreams();
        }
    }
}