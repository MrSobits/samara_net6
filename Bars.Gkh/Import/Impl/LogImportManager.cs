namespace Bars.Gkh.Import
{
    using System;
    using System.IO;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Tasks.Common.Entities;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums.Import;

    using Castle.MicroKernel.Lifestyle;
    using Castle.Windsor;

    using Ionic.Zip;
    using Ionic.Zlib;

    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Сервис для работы с <see cref="LogImport"/>
    /// </summary>
    public class LogImportManager : ILogImportManager
    {
        private ZipFile logsZip;

        private ZipFile filesZip;

        private B4.Modules.FileStorage.FileInfo fileLog;

        /// <summary>
        /// Key импорта
        /// </summary>
        private string importKey;

        /// <summary>
        /// общее количество измененных строк
        /// </summary>
        private int сountChangedRows;

        /// <summary>
        /// общее количество созданных строк
        /// </summary>
        private int countImportedRows;

        /// <summary>
        /// Импортируемый файл
        /// </summary>
        private B4.Modules.FileStorage.FileInfo importedFile;

        /// <summary>
        /// Лог импорта
        /// </summary>
        private LogImport logImport;

        private bool disposed;

        /// <summary>
        /// .ctor
        /// </summary>
        public LogImportManager()
        {
            this.logsZip = new ZipFile(Encoding.UTF8)
            {
                CompressionLevel = CompressionLevel.Level3,
            };

            this.filesZip = new ZipFile(Encoding.UTF8)
            {
                CompressionLevel = CompressionLevel.Level3,
            };
        }

        /// <summary>
        /// Деструктор
        /// </summary>
        ~LogImportManager()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// IoC
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// общее количество ошибок
        /// </summary>
        public int CountError { get; private set; }

        /// <summary>
        /// общее количество предупреждений
        /// </summary>
        public int CountWarning { get; private set; }

        /// <summary>
        /// Идентификатор файла
        /// </summary>
        public long LogFileId { get; private set; }

        /// <summary>
        /// Название файла
        /// </summary>
        public string FileNameWithoutExtention { get; set; }

        /// <summary>
        /// Количество импортируемых файлов
        /// </summary>
        public int CountImportedFile { get; set; }

        /// <summary>
        /// Дата загрузки
        /// </summary>
        public DateTime? UploadDate { get; set; }

        /// <summary>
        /// Задача, которая разбирала импорт, на сервере вычислений
        /// </summary>
        public TaskEntry Task { get; set; }

        /// <inheritdoc />
        public bool LogImportPreviewMode { get; set; }

        /// <inheritdoc />
        public void Update(ILogImport log) => this.SetLogInfo(log);

        /// <summary>
        /// Добавляем импортируемый файл и его лог
        /// </summary>
        /// <param name="file">Импортируемый файл</param>
        /// <param name="fileName">Наименование файла с расширением</param>
        /// <param name="log"></param>
        public void Add(Stream file, string fileName, ILogImport log)
        {
            this.ThrowIfDisposed();

            if (fileName.Split('.').Length == 0)
            {
                throw new Exception("Не задано расширение файла");
            }

            this.CountImportedFile++;
            this.AddFile(file, fileName);
            this.AddLogInternal(log);
        }

        /// <summary>
        /// Добавить
        /// </summary>
        public void Add(FileData file, ILogImport log)
        {
            this.ThrowIfDisposed();

            using (var stream = new MemoryStream(file.Data))
            {
                this.Add(stream, $"{file.FileName}.{file.Extention}", log);
            }
        }

        /// <summary>
        /// Добавляем импортируемый файл и его лог
        /// </summary>
        /// <param name="file">Импортируемый файл</param>
        /// <param name="fileName">Наименование файла с расширением</param>
        /// <param name="log"></param>
        public void Add(B4.Modules.FileStorage.FileInfo file, ILogImport log)
        {
            this.importedFile = file;
            this.CountImportedFile++;
            this.AddLogInternal(log);
        }

        /// <inheritdoc />
        public void Add(FileData importFile, B4.Modules.FileStorage.FileInfo fileLog, ILogImport log)
        {
            this.ThrowIfDisposed();
            using (var stream = new MemoryStream(importFile.Data))
            {
                this.AddFile(stream, $"{importFile.FileName}.{importFile.Extention}");
            }
            this.CountImportedFile++;
            this.fileLog = fileLog;
            this.AddLogInternal(log);
        }

        /// <summary>
        /// Добавляем лог, использовать если не нужно добавлять сам файл импорта
        /// </summary>
        /// <param name="log"></param>
        public void AddLog(ILogImport log)
        {
            this.ThrowIfDisposed();

            this.CountImportedFile++;
            this.AddLogInternal(log);
        }

        /// <summary>
        /// Сохранить лог в систему
        /// </summary>
        public long Save()
        {
            this.ThrowIfDisposed();

            using var oldScope = this.Container.BeginScope();

            var fileManager = this.Container.Resolve<IFileManager>();
            var repLogImport = this.Container.Resolve<IDomainService<LogImport>>();

            try
            {
                using (var logFile = new MemoryStream())
                {
                    this.logsZip.Save(logFile);

                    this.SaveImportedFile(fileManager);

                    // Сохраняем логи в файловом хранилище
                    var logFileInfo = this.fileLog.IsNotNull()
                        ? this.fileLog
                        : fileManager.SaveFile(logFile, $"{this.FileNameWithoutExtention}.log.zip");

                    if (this.logImport == null)
                    {
                        this.logImport = this.GetNewLogImportEntity();
                    }
                    else
                    {
                        this.logImport.UploadDate = DateTime.Now;
                    }

                    this.logImport.CountImportedFile = this.CountImportedFile;
                    this.logImport.CountImportedRows = this.countImportedRows;
                    this.logImport.CountChangedRows = this.сountChangedRows;
                    this.logImport.CountError = this.CountError;
                    this.logImport.CountWarning = this.CountWarning;
                    this.logImport.File = this.importedFile;
                    this.logImport.LogFile = logFileInfo;
                    this.logImport.FileName = this.FileNameWithoutExtention;

                    repLogImport.SaveOrUpdate(this.logImport);

                    this.LogFileId = logFileInfo.Id;
                }
            }
            catch (Exception exp)
            {
                this.Container.Resolve<ILogger>().LogError(exp, "Произошла ошибка при создании лога импорта".Localize());
            }
            finally
            {
                this.Container.Release(fileManager);
                this.Container.Release(repLogImport);

                this.Dispose();
            }

            return this.LogFileId;
        }

        /// <summary>
        /// Получить новую сущность лога имопрта
        /// </summary>
        private LogImport GetNewLogImportEntity()
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();

            var activeOperator = userManager.GetActiveOperator();
            var login = userManager.GetActiveUser()?.Login ?? "anonymous";

            return new LogImport
            {
                UploadDate = this.UploadDate ?? DateTime.Now,
                CountImportedFile = this.CountImportedFile,
                File = this.importedFile,
                FileName = this.FileNameWithoutExtention,
                ImportKey = this.importKey,
                Operator = activeOperator,
                Login = login,
                Task = this.Task
            };
        }

        /// <summary>
        /// Сохранить импортируемый файл
        /// </summary>
        private void SaveImportedFile(IFileManager fileManager = null)
        {
            if (this.importedFile == null)
            {
                var needFileManagerResolve = fileManager == null;

                if (needFileManagerResolve)
                {
                    fileManager = this.Container.Resolve<IFileManager>();
                }

                using (var file = new MemoryStream())
                {
                    this.filesZip.Save(file);

                    this.importedFile = fileManager.SaveFile(file, $"{this.FileNameWithoutExtention}.zip");
                }

                if (needFileManagerResolve)
                {
                    this.Container.Release(fileManager);
                }
            }
        }

        /// <summary>
        /// Получить информацию
        /// </summary>
        public string GetInfo()
        {
            var msg = string.Empty;

            if (this.CountImportedFile > 1)
            {
                msg = $"Количество загруженных файлов:{this.CountImportedFile};";
            }

            msg += $"Загружено:{this.countImportedRows}; Изменено: {this.сountChangedRows}; Предупреждений: {this.CountWarning}; Ошибок: {this.CountError}";

            return msg;
        }

        /// <summary>
        /// Вернуть статус импорта
        /// </summary>
        /// <returns></returns>
        public StatusImport GetStatus()
        {
            return this.CountError > 0
                ? StatusImport.CompletedWithError
                : (this.CountWarning > 0 ? StatusImport.CompletedWithWarning : StatusImport.CompletedWithoutError);
        }

        /// <summary>
        /// Освободить
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// Добавляем импортируемый файл
        /// </summary>
        private void AddFile(Stream file, string fileName)
        {
            file.Seek(0, SeekOrigin.Begin);

            var buffer = new byte[file.Length];

            file.Seek(0, SeekOrigin.Begin);
            file.Read(buffer, 0, buffer.Length);
            file.Seek(0, SeekOrigin.Begin);

            this.filesZip.AddEntry(fileName, buffer);
        }

        /// <summary>
        /// Указать информацию об имопрте из ILogImport
        /// </summary>
        private void SetLogInfo(ILogImport log)
        {
            log.PlacingResults();

            this.сountChangedRows += log.CountChangedRows;
            this.countImportedRows += log.CountAddedRows;
            this.CountError += log.CountError;
            this.CountWarning += log.CountWarning;
        }

        private void AddLogInternal(ILogImport log)
        {
            this.SetLogInfo(log);

            if (string.IsNullOrEmpty(log.ImportKey))
            {
                throw new Exception("Не задан key импорта");
            }

            this.importKey = log.ImportKey;

            var fileStream = log.GetFile();
            var buffer = new byte[fileStream.Length];

            fileStream.Seek(0, SeekOrigin.Begin);
            fileStream.Read(buffer, 0, buffer.Length);
            fileStream.Seek(0, SeekOrigin.Begin);

            this.logsZip.AddEntry(string.IsNullOrEmpty(log.FileName) ? this.FileNameWithoutExtention + ".log" : log.FileName, buffer);

            var errorRowsStream = log.GetErrorRows();
            if (errorRowsStream.IsNotNull())
            {
                errorRowsStream.Seek(0, SeekOrigin.Begin);
                this.logsZip.AddEntry(this.FileNameWithoutExtention + "_errors.csv", errorRowsStream);
            }

            if (this.LogImportPreviewMode)
            {
                this.SaveImportedFile();

                var logImportDomain = this.Container.ResolveDomain<LogImport>();

                using (this.Container.Using(logImportDomain))
                {
                    this.logImport = this.GetNewLogImportEntity();

                    logImportDomain.Save(this.logImport);
                }
            }
        }

        private void Dispose(bool disposing)
        {
            if (this.disposed)
                return;

            if (disposing)
            {
                if (this.logsZip != null)
                {
                    this.logsZip.Dispose();
                    this.logsZip = null;
                }
                if (this.filesZip != null)
                {
                    this.filesZip.Dispose();
                    this.filesZip = null;
                }
            }

            this.disposed = true;
        }

        private void ThrowIfDisposed()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException("LogImportManager", "Log manager is disposed");
            }
        }
    }
}