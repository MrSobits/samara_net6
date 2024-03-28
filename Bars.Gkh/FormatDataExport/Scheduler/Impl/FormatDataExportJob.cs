namespace Bars.Gkh.FormatDataExport.Scheduler.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.Config;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.ConfigSections.Administration;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Administration.FormatDataExport;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.FormatProvider;
    using Bars.Gkh.FormatDataExport.FormatProvider.CsvFormat;
    using Bars.Gkh.FormatDataExport.NetworkWorker.Responses;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Задача запускаемая при срабатывании триггера
    /// </summary>
    public class FormatDataExportJob
    {
        private string FtpPath => ApplicationContext.Current.Configuration.AppSettings.GetAs<string>("FtpPath");
        private FormatDataExportResult ExportResult { get; set; }
        private FormatDataExportRemoteResult ExportRemoteResult { get; set; }

        public FormatDataExportTask ExportTask { private get; set; }
        public CancellationToken CancellationToken { private get; set; }
        public IProgressIndicator ProgressIndicator { private get; set; }

        public IWindsorContainer Container { get; set; }
        public IExportFormatProviderBuilder ExportProviderBuilder { get; set; }
        public IGkhUserManager GkhUserManager { get; set; }
        public IConfigProvider ConfigProvider { get; set; }
        public ILogger LogManager { get; set; }
        public IDomainService<FormatDataExportTask> FormatDataExportTaskDomain { get; set; }
        public IDomainService<FormatDataExportResult> FormatDataExportResultDomain { get; set; }
        public IDomainService<FormatDataExportRemoteResult> FormatDataExportRemoteResultDomain { get; set; }

        /// <summary>
        /// Запуск экспорта
        /// </summary>
        public void Execute()
        {
            this.ExportResult = new FormatDataExportResult
            {
                StartDate = DateTime.Now,
                Task = this.ExportTask,
                Progress = 0,
                Status = FormatDataExportStatus.Running,
                EntityCodeList = this.GetEntityCodeList(this.ExportTask.EntityGroupCodeList)
            };
            this.UpdateResult();

            var activeUser = this.GkhUserManager.GetActiveUser();
            var activeOperatorLogin = activeUser?.Login;
            var baseParams = this.ExportTask.BaseParams ?? new BaseParams();
            string pathToSave = "";
            
            var logOperation = new LogOperation
            {
                User = activeUser,
                StartDate = DateTime.Now,
                OperationType = LogOperationType.FormatDataExport,
                Comment = "Ошибки при экспорте данных в РИС ЖКХ"
            };

            IExportFormatProvider exportProvider = null;
            try
            {
                baseParams.Params.Apply(new Dictionary<string, object>
                {
                    { "UserId", activeUser?.Id },
                });

                var exportProviderBuilder = this.ExportProviderBuilder
                    .SetParams(baseParams)
                    .SetLogOperation(logOperation)
                    .SetCancellationToken(this.CancellationToken)
                    .SetEntytyGroupCodeList(this.ExportTask.EntityGroupCodeList);

                exportProvider = this.CheckRemoteAddress()
                    ? exportProviderBuilder.Build<NetCsvFormatProvider>()
                    : exportProviderBuilder.Build<CsvFormatProvider>();

                logOperation = exportProvider.LogOperation;
                this.ExportResult.EntityCodeList = exportProvider.EntityCodeList;
                this.UpdateResult();

                this.ExportRemoteResult = new FormatDataExportRemoteResult
                {
                    TaskResult = this.ExportResult
                };
                this.UpdateRemoteResult();

                if (this.ExportTask.IsDelete)
                {
                    throw new OperationCanceledException("Задача удалена");
                }

                exportProvider.OnProgressChanged += this.ExportProviderOnProgressChanged;
                exportProvider.OnAfterExport += this.ExportProviderOnAfterExport;

                var versionFolder = $"format_{exportProvider.FormatVersion}";
                var contragentId = baseParams.Params.GetAs<long>("MainContragent");
                var fileName = $"{DateTime.Now:yyyyMMdd_HHmmss}_{(contragentId != 0 ? contragentId.ToString() : Guid.NewGuid().ToString("N"))}.zip";

                var ftpDirectory = this.GetFtpDirectory(versionFolder, activeOperatorLogin);
                if (!Directory.Exists(ftpDirectory))
                {
                    Directory.CreateDirectory(ftpDirectory);
                }

                pathToSave = Path.Combine(ftpDirectory, fileName);

                this.ParseRemoteResult(exportProvider.Export(pathToSave));

                this.ExportResult.Progress = 100;
                this.ExportResult.Status = string.IsNullOrEmpty(exportProvider.SummaryErrors)
                    ? FormatDataExportStatus.Successfull
                    : FormatDataExportStatus.Error;
            }
            catch (OperationCanceledException)
            {
                var error = "Экспорт данных в РИС ЖКХ прерван пользователем";
                this.LogManager.LogDebug(error);
                this.ExportResult.Status = FormatDataExportStatus.Aborted;
                this.ExportResult.Progress = 0;
                this.SaveLogFile(logOperation, error);
            }
            catch (Exception e)
            {
                this.LogManager.LogError(e, "Экспорт данных в РИС ЖКХ");
                this.ExportResult.Status = FormatDataExportStatus.RuntimeError;
                this.ExportResult.Progress = 0;
                if (logOperation.LogFile == null)
                {
                    this.SaveLogFile(logOperation, e.ToString());
                }
            }
            finally
            {
                this.ExportResult.LogOperation = logOperation;
                this.ExportResult.EndDate = DateTime.Now;
                this.UpdateResult();
                this.Container.Release(exportProvider);
            }
        }

        private void ExportProviderOnAfterExport(object sender, ICollection<string> exportedEntityCodeList)
        {
            this.ExportResult.EntityCodeList = exportedEntityCodeList.ToList();
            this.UpdateResult();
        }

        private bool CheckRemoteAddress()
        {
            var config = this.Container.GetGkhConfig<AdministrationConfig>()
                .FormatDataExport
                .FormatDataExportGeneral;

            return !string.IsNullOrWhiteSpace(config.TransferServiceAddress) && !string.IsNullOrWhiteSpace(config.TransferServiceToken);
        }

        private void ExportProviderOnProgressChanged(object sender, float progress)
        {
            this.ProgressIndicator?.Report(null, (uint)Math.Round(progress), string.Empty);
            this.ExportResult.Progress = progress;
            this.UpdateResult();
        }

        private void UpdateResult()
        {
            this.FormatDataExportResultDomain.SaveOrUpdate(this.ExportResult);
        }

        private void UpdateRemoteResult()
        {
            this.FormatDataExportRemoteResultDomain.SaveOrUpdate(this.ExportRemoteResult);
        }

        private string GetFtpDirectory(string formatFolder, string userName)
        {
            return Path.Combine(this.FtpPath, formatFolder, userName);
        }

        private void SaveLogFile(LogOperation logOperation, string error)
        {
            this.Container.UsingForResolved<IFileManager>((container, fileManager) =>
            {
                using (var memoryStream = new MemoryStream())
                using (var writer = new StreamWriter(memoryStream))
                {
                    writer.Write(error);
                    writer.Flush();

                    logOperation.LogFile = fileManager.SaveFile(memoryStream, $"{logOperation.OperationType.GetEnumMeta().Display}.log");
                }
            });

            logOperation.EndDate = DateTime.Now;

            this.Container.UsingForResolved<IRepository<LogOperation>>((container, repository) =>
            {
                repository.SaveOrUpdate(logOperation);
            });
        }

        private IList<string> GetEntityCodeList(ICollection<string> entityGroupCodeList)
        {
            var result = new HashSet<string>();

            this.Container.UsingForResolvedAll<IExportableEntityGroup>((ioc, groups) =>
            {
                result = groups.Where(x => entityGroupCodeList.Contains(x.Code))
                    .SelectMany(x => x.InheritedEntityCodeList)
                    .ToHashSet();
            });

            return result.ToList();
        }

        private void ParseRemoteResult(IDataResult dataResult)
        {
            var remoteResult = dataResult?.Data as FormatDataExportRemoteResult;
            if (remoteResult == null)
            {
                return;
            }

            this.ExportRemoteResult.FileId = remoteResult.FileId;
            this.ExportRemoteResult.TaskId = remoteResult.TaskId;
            this.ExportRemoteResult.Status = remoteResult.Status;
            this.ExportRemoteResult.UploadResult = remoteResult.UploadResult;

            this.UpdateRemoteResult();
        }
    }
}