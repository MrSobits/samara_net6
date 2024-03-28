namespace Bars.Gkh.RegOperator.Imports
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums.Import;
    using Bars.Gkh.Import;
    using Bars.Gkh.Import.Impl;
    using Bars.Gkh.RegOperator.DomainService;
    using Bars.Gkh.RegOperator.Tasks.BankDocumentImport;
    using Bars.Gkh.RegOperator.Tasks.TaskHelpers;

    using SharpCompress.Archives;

    using SharpCompressZipArchive = SharpCompress.Archives.Zip.ZipArchive;

    /// <summary>
    /// Импорт архива с документами из банка
    /// </summary>
    public class BankArchiveImport: GkhImportBase
    {
        private static readonly string[] ArchiveExtensions = { "zip", "rar", "7z", "tar", "gz" };

        /// <summary>
        /// Проверяет поддерживается ли файл архива по расширению
        /// </summary>
        public static bool IsSupportArchive(FileData file)
        {
            return Enumerable.Contains(BankArchiveImport.ArchiveExtensions, file.Extention.ToLower());
        }

        private readonly int unpackPercentValue = 90; // % на распаковку
        private readonly int createTaskPercentValue = 10; // % на постановку задачи

        /// <summary>
        /// Лог менеджер
        /// </summary>
        public ILogImportManager LogManager { get; set; }

        /// <summary>
        /// Провайдер импорта документа из банка
        /// </summary>
        public IBankDocumentImportProvider BankDocumentImportProvider { get; set; }

        /// <summary>
        /// Код для регистрации
        /// </summary>
        public static string Id => typeof(BankArchiveImport).FullName;

        /// <summary>
        /// Ключ импорта
        /// </summary>
        public override string Key => BankArchiveImport.Id;

        /// <summary>
        /// Код группировки импорта (например группировка в меню)
        /// </summary>
        public override string CodeImport => "BankArchive";

        /// <summary>
        /// Наименование импорта
        /// </summary>
        public override string Name => "Распаковка архива с реестрами оплат";

        /// <summary>
        /// Разрешенные расширения файлов
        /// </summary>
        public override string PossibleFileExtensions => string.Empty;

        /// <summary>
        /// Права
        /// </summary>
        public override string PermissionName => "Import.BankArchive";

        /// <summary>
        /// Импортировать
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат импорта</returns>
        public override ImportResult Import(BaseParams baseParams)
        {
            try
            {
                var fileImport = baseParams.Files["FileImport"];
                var taskParams = new BaseParams() { Params = baseParams.Params };
                IList<FileData> unpackingFiles;

                if (this.IsZipFile(fileImport))
                {
                    unpackingFiles = this.UnpackZipArchive(fileImport);
                }
                else
                {
                    unpackingFiles = this.UnPackOtherArchive(fileImport);
                }

                if (unpackingFiles.IsNotEmpty())
                {
                    var progress = new ProgressSender(unpackingFiles.Count(),
                        this.Indicate,
                        this.createTaskPercentValue,
                        this.unpackPercentValue);

                    this.Container.UsingForResolved<ITaskManager>((container, taskManager) =>
                    {
                        foreach (var file in unpackingFiles)
                        {
                            taskParams.Files = this.GetFileByBaseParams(file);
                            
                            taskManager.CreateTasks(new BankDocumentImportFileProvider(Container), taskParams);

                            progress.TrySend();
                        }
                    });
                    return new ImportResult() {Success = true};
                }
                else
                {
                    return new ImportResult(StatusImport.CompletedWithWarning, "Архив пуст") {Success = false};
                }

            }
            catch (Exception ex)
            {
                return new ImportResult(StatusImport.CompletedWithError, ex.Message) { Success = false };
            }
        }

        private IDictionary<string, FileData> GetFileByBaseParams(FileData file)
        {
            return new Dictionary<string, FileData>() { { "FileImport", file } };
        }

        private IList<FileData> UnPackOtherArchive(FileData fileData)
        {
            IList<FileData> unpackingFiles = new List<FileData>();
            using (var archMemoryStream = new MemoryStream(fileData.Data))
            {
                using (var archive = ArchiveFactory.Open(archMemoryStream))
                {
                    var progress = new ProgressSender(archive.Entries.Count(),
                        this.Indicate,
                        this.unpackPercentValue);

                    foreach (var archiveEntry in archive.Entries.Where(x => !x.IsDirectory))
                    {
                        using (var ms = new MemoryStream())
                        {
                            archiveEntry.WriteTo(ms);
                            ms.Seek(0, SeekOrigin.Begin);

                            unpackingFiles.Add(new FileData(
                                Path.GetFileNameWithoutExtension(archiveEntry.Key),
                                Path.GetExtension(archiveEntry.Key),
                                ms.ReadAllBytes()));

                            progress.TrySend();

                        }
                    }
                }
            }

            return unpackingFiles;
        }

        private IList<FileData> UnpackZipArchive(FileData fileData)
        {
            IList<FileData> unpackingFiles = new List<FileData>();
            using (var archMemoryStream = new MemoryStream(fileData.Data))
            {
                using (var zip = new ZipArchive(archMemoryStream, ZipArchiveMode.Read))
                {
                    var progress = new ProgressSender(zip.Entries.Count(),
                        this.Indicate,
                        this.unpackPercentValue);
                    zip.Entries.Where(x => x.Name.Length > 0).ForEach(e =>
                    {
                        using (var ms = new MemoryStream())
                        {
                            var filename = this.DecodeZipFileName(e.Name);

                            using (var entryStream = e.Open())
                            {
                                if (entryStream != null && entryStream.CanRead)
                                {
                                    entryStream.CopyTo(ms);
                                }
                            }
                            ms.Seek(0, SeekOrigin.Begin);
                            unpackingFiles.Add(new FileData(
                                Path.GetFileNameWithoutExtension(filename),
                                Path.GetExtension(filename),
                                ms.ReadAllBytes()));

                            progress.TrySend();
                        }
                    });
                }
            }

            return unpackingFiles;
        }

        private bool IsZipFile(FileData file)
        {
            using (var ms = new MemoryStream(file.Data))
            {
                if (ms.CanRead)
                {
                    return SharpCompressZipArchive.IsZipFile(ms);
                }
                else
                {
                    return false;
                }
            }
        }

        // Декодирует имена файлов в кирилице windows-1251 - cp866
        private string DecodeZipFileName(string fileName)
        {
            var cp866 = Encoding.GetEncoding(866);
            var win1251 = Encoding.GetEncoding(1251);

            return cp866.GetString(win1251.GetBytes(fileName));
        }
    }
}