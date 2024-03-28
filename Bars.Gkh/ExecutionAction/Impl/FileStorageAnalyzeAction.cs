namespace Bars.Gkh.ExecutionAction.Impl
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using Bars.B4;
    using Bars.B4.Config;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Utils;

    using Newtonsoft.Json;

    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;

    public class FileStorageAnalyzeAction : BaseExecutionAction
    {
        private readonly string tempFolder = ".tempfiles";
        private readonly string fileStorageDirectory;
        private readonly int portion = 20000;
        private readonly LogEntity log;
        private IEnumerable<string> directoryFiles;
        private string dateTempFolder;

        /// <inheritdoc />
        public override string Name => "Анализ файлового хранилища";
        
        /// <inheritdoc />
        public override string Description => $"Анализ файлового хранилища. Перенос файлов с отсутствующими на них ссылками в БД в папку .tempfiles для последующей проверки и ручной очистки.";

        /// <inheritdoc />
        public override Func<IDataResult> Action => this.Execute;

        public FileStorageAnalyzeAction(IConfigProvider configProvider)
        {
            this.fileStorageDirectory = configProvider.GetConfig().ModulesConfig["Bars.B4.Modules.FileSystemStorage"].GetAs("FileDirectory", string.Empty);
            this.log = new LogEntity();
        }

        private IDataResult Execute()
        {
            try
            {
                this.FillMovingFilesDictionary();
                this.MoveFiles();
            }
            catch (Exception ex)
            {
                var result = new BaseDataResult{Success = false};
                    
                if (ex is AggregateException aggregateException)
                {
                    result.Message = aggregateException.Message + "; " + string.Join("; ", aggregateException.InnerExceptions.Select(x => x.Message));
                }
                else
                {
                    result.Message = ex.Message + "; " + ex.InnerException?.Message;
                }

                if (log.Count > 0)
                {
                    result.Data = log;
                }

                return result;
            }

            return new BaseDataResult(log);
        }

        // Заполнение словаря перемещаемых файлов
        private void FillMovingFilesDictionary()
        {
            var periodStart = DateTime.Parse(this.ExecutionParams.Params.GetAs<string>("Month")).GetFirstDayOfMonth();
            var periodEnd = periodStart.WithLastDayMonth().AddDays(1).AddTicks(-1);
            var filesDirectoryPath = Path.Combine(this.fileStorageDirectory, periodStart.Year.ToString(), periodStart.Month.ToString());
            var fileInfoDomain = this.Container.ResolveDomain<FileInfo>();

            using (this.Container.Using(fileInfoDomain))
            {
                var query = fileInfoDomain.GetAll()
                    .Where(x => x.ObjectCreateDate >= periodStart && x.ObjectCreateDate <= periodEnd);
                var count = query.Count();
                var skipValue = 0;

                this.dateTempFolder = Path.Combine(this.fileStorageDirectory, this.tempFolder, periodStart.Year.ToString(), periodStart.Month.ToString());
                this.directoryFiles = Directory.EnumerateFiles(filesDirectoryPath);

                while (skipValue < count)
                {
                    var filesPaths = query
                        .OrderBy(x => x.Id)
                        .Skip(skipValue)
                        .Take(this.portion)
                        .Select(x =>
                            $"{Path.Combine(this.fileStorageDirectory, x.ObjectCreateDate.Year.ToString(), x.ObjectCreateDate.Month.ToString())}\\{x.Id}.{x.Extention}")
                        .ToList();
                    
                    this.directoryFiles = this.directoryFiles.Except(filesPaths);
                    
                    skipValue += this.portion;
                }
            }
        }

        // Перемещение файлов
        private void MoveFiles()
        {
            var loopExceptions = new ConcurrentQueue<Exception>();

            Parallel.ForEach(this.directoryFiles, 
                x => 
                {
                    try
                    {
                        if (!File.Exists(x))
                        {
                            return;
                        }
                        
                        var fileName = Path.GetFileName(x);
                        var newFilePath = Path.Combine(this.dateTempFolder, fileName);

                        Directory.CreateDirectory(this.dateTempFolder);

                        if (File.Exists(newFilePath))
                        {
                            File.Delete(newFilePath);
                        }

                        File.Move(x, newFilePath);

                        log.Details.Add(new Detail { Name = fileName, Path = this.dateTempFolder });
                    }
                    catch (Exception ex)
                    {
                        loopExceptions.Enqueue(ex);
                    }
                });
                    
            log.Count = log.Details.Count;

            if (!loopExceptions.IsEmpty)
            {
                throw new AggregateException(loopExceptions);
            }
        }
        
        // Вспомогательная сущность для логирования
        private class LogEntity
        {
            public LogEntity()
            {
                this.Details = new ConcurrentBag<Detail>();
            }
            
            [JsonProperty("detail", Order = 1)]
            public ConcurrentBag<Detail> Details { get; set; }
            
            [JsonProperty("count")]
            public int Count { get; set; }
        }
        
        // Детали перемещенных файлов
        private class Detail
        {
            [JsonProperty("path")]
            public string Path { get; set; }
            
            [JsonProperty("name")]
            public string Name { get; set; }
        }

    }
}