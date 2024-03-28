namespace Bars.B4.Modules.Analytics.Reports.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;

    using Bars.B4.Config;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.B4.Modules.Analytics.Enums;
    using Bars.B4.Modules.Analytics.Reports.Entities;
    using Bars.B4.Modules.Analytics.Reports.Entities.History;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Analytics.Reports.Extensions;
    using Bars.B4.Modules.Analytics.Reports.Generators;
    using Bars.B4.Modules.Analytics.Reports.ReportHandlers;
    using Bars.B4.Modules.Analytics.Reports.Tasks;
    using Bars.B4.Modules.Analytics.Reports.Utils;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Tasks.Common.Contracts;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Utils;

    using Castle.Windsor;

    using Microsoft.Extensions.Logging;

    using Newtonsoft.Json;

    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;

    /// <summary>
    /// Сервис генерации отчётов
    /// </summary>
    public class ReportGeneratorService : IReportGeneratorService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Генератор отчётов
        /// </summary>
        public IReportGenerator ReportGenerator { get; set; }

        /// <summary>
        /// Менеджер файлов
        /// </summary>
        public IFileManager FileManager { get; set; }

        /// <summary>
        /// Менеджер задач
        /// </summary>
        public ITaskManager TaskManager { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="StoredReport"/>
        /// </summary>
        public IDomainService<StoredReport> StoredReportDomain { get; set; }

        /// <summary>
        /// Поставщик информации о пользователе
        /// </summary>
        public IUserInfoProvider UserInfoProvider { get; set; }

        public IDomainService<ReportHistory> ReportHistoryDomain { get; set; }

        /// <summary>
        /// Менеджер логов
        /// </summary>
        public ILogger LogManager { get; set; }

        /// <inheritdoc />
        public ReportResult Generate(BaseParams baseParams)
        {
            var reportId = baseParams.Params.GetAs<long>("reportId", ignoreCase: true);
            var format = baseParams.Params.GetAs("format", ignoreCase: true, defaultValue: ReportPrintFormat.xls);
            var report = this.StoredReportDomain.FirstOrDefault(x => x.Id == reportId);

            var file = this.GenerateReportInternal(baseParams, report, format);

            return new ReportResult
            {
                ReportStream = file,
                FileName = $"{report.Name}.{format.Extension()}"
            };
        }

        /// <inheritdoc />
        public IDataResult CreateTaskOrSaveOnServer(BaseParams baseParams)
        {
            var beforeResult = this.BeforeGenerate(baseParams);
            if (!beforeResult.Success)
            {
                return beforeResult;
            }

            var reportId = baseParams.Params.GetAs<long>("reportId", ignoreCase: true);
            var report = this.StoredReportDomain.FirstOrDefault(x => x.Id == reportId);
            var format = baseParams.Params.GetAs("format", ignoreCase: true, defaultValue: ReportPrintFormat.xls);

            if (report.GenerateOnCalcServer)
            {
                this.SendStatus(baseParams, TaskStatus.InProgress);
                var taskResult = this.TaskManager.CreateTasks(new ReportGeneratorTask(report.Name), baseParams);

                // возвращаем результат постановки задачи
                return new BaseDataResult(new
                {
                    taskedReport = true,
                    taskId = taskResult.Data.ParentTaskId
                });
            }

            var result = this.SaveOnServer(baseParams, report, format);

            return new BaseDataResult(new
            {
                taskedReport = false,
                fileId = result.Data.Id
            });
        }

        /// <inheritdoc />
        public IDataResult<FileInfo> SaveOnServer(BaseParams baseParams, StoredReport report, ReportPrintFormat format)
        {
            IDataResult<FileInfo> reportResult = null;

            using (var file = this.GenerateReportInternal(baseParams, report, format))
            {
                var fileName = (file as FileStream)?.Name;

                using (var transaction = this.Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        var result = this.FileManager.SaveFile(file, this.GetFileName(report, format));

                        var paramsForSave = baseParams.Params.GetAs<Dictionary<string, object>>("paramsForSave", ignoreCase: true);

                        var history = new ReportHistory()
                        {
                            ReportType = ReportType.StoredReport,
                            ReportId = report.Id,
                            Date = DateTime.UtcNow,
                            Category = report.Category,
                            Name = report.DisplayName,
                            File = result,
                            User = this.UserInfoProvider?.GetActiveUser()
                        };

                        var reportParams = report.GetParams().ToDictionary(x => x.Name);

                        paramsForSave.ForEach(
                            x =>
                            {
                                var param = reportParams.Get(x.Key);
                                if (param != null)
                                {
                                    var dict = (DynamicDictionary) x.Value;

                                    history.ParameterValues.Add(
                                        x.Key,
                                        new ReportHistoryParam
                                        {
                                            DisplayName = param.Label,
                                            Value = this.GetValue(param, dict),
                                            DisplayValue = this.GetDisplayValue(param, dict)
                                        });
                                }
                            });
                        this.ReportHistoryDomain.Save(history);

                        transaction.Commit();

                        reportResult = new GenericDataResult<FileInfo>(result);
                        return reportResult;
                    }
                    catch
                    {
                        if (report.GenerateOnCalcServer)
                        {
                            this.SendStatus(baseParams, TaskStatus.Error);
                        }
                        transaction.Rollback();
                        throw;
                    }
                    finally
                    {
                        if (!string.IsNullOrEmpty(fileName) && format == ReportPrintFormat.zip && File.Exists(fileName))
                        {
                            File.Delete(fileName);
                        }

                        this.AfterGenerate(baseParams, reportResult, report.GenerateOnCalcServer);
                    }
                }
            }
        }

        private object GetValue(IParam param, DynamicDictionary paramValueObject)
        {
            return param.Multiselect && paramValueObject.GetAs<string>("Value") != "All"
                ? paramValueObject.GetAs<List<object>>("Value")
                : paramValueObject.Get("Value");
        }

        private string GetDisplayValue(IParam param, DynamicDictionary paramValueObject)
        {
            return param.ParamType == ParamType.Bool
                ? (bool)paramValueObject["Value"]
                    ? "Да"
                    : "Нет"
                : (string)paramValueObject["DisplayValue"];
        }

        private Stream GenerateReportInternal(BaseParams baseParams, StoredReport report, ReportPrintFormat format)
        {
            var guid = Guid.NewGuid();

            try
            {
                var sw = Stopwatch.StartNew();
                this.LogManager.LogInformation($"Запуск отчета: {guid}; Название: {report.DisplayName}; Пользователи: {this.GetActiveUserName()}");

                var file = this.ReportGenerator.Generate(
                    report,
                    report.GetTemplate(),
                    baseParams,
                    format,
                    new Dictionary<string, object>
                    {
                        { "ExportSettings", report.GetExportSettings(format) },
                        { "UseTemplateConnectionString", report.UseTemplateConnectionString }
                    });

                this.LogManager.LogInformation($"Конец формирования отчета: {guid}; Название: {report.DisplayName}; Время формирования: {sw.Elapsed}");

                file.Seek(0, SeekOrigin.Begin);
                return file;
            }
            catch
            {
                if (report.GenerateOnCalcServer)
                {
                    this.SendStatus(baseParams, TaskStatus.Error);
                }

                // Текст ошибки в error.log
                this.LogManager.LogInformation($"Ошибка формирования отчета: {guid}; Название: {report.DisplayName}");

                throw;
            }
            
        }

        private string GetActiveUserName()
        {
            return this.UserInfoProvider?.GetActiveUser()?.Name;
        }

        private string GetFileName(StoredReport report, ReportPrintFormat format)
        {
            var name = string.Join("_", report.Name.Split(Path.GetInvalidFileNameChars()));
            var ext = format.Extension();

            return $"{name}.{ext}";
        }

        private IDataResult BeforeGenerate(BaseParams baseParams)
        {
            var codeReport = baseParams.Params.GetAs("codeReport", string.Empty, true);
            if (string.IsNullOrEmpty(codeReport))
            {
                return new BaseDataResult("Не указан код отчета");
            }
            var result = new BaseDataResult();
            this.Container.UsingForResolvedAll<IReportHandler>((ioc, handlers) =>
            {
                var handlerResult = new List<IDataResult>();
                handlers.Where(x => x.Code == codeReport).ForEach(x => handlerResult.Add(x.BeforePrint(baseParams)));

                result.Success = handlerResult.All(x => x.Success);
                result.Data = handlerResult;
            });

            return result;
        }

        private IDataResult AfterGenerate(BaseParams baseParams, IDataResult<FileInfo> reportResult, bool generateOnCalcServer)
        {
            var codeReport = baseParams.Params.GetAs("codeReport", string.Empty, true);

            if ((reportResult?.Success ?? false) && generateOnCalcServer)
            {
                baseParams.Params.SetValue("fileId", reportResult.Data.Id);
                this.SendStatus(baseParams, TaskStatus.Succeeded);
            }

            if (string.IsNullOrEmpty(codeReport))
            {
                return new BaseDataResult("Не указан код отчета");
            }
            var result = new BaseDataResult();
            this.Container.UsingForResolvedAll<IReportHandler>((ioc, handlers) =>
            {
                var handlerResult = new List<IDataResult>();
                handlers.Where(x => x.Code == codeReport).ForEach(x => handlerResult.Add(x.AfterPrint(baseParams, reportResult)));

                result.Success = handlerResult.All(x => x.Success);
                result.Data = handlerResult;
            });

            return result;
        }

        /// <summary>
        /// Отправляет статус задачи.
        /// </summary>
        /// <param name="baseParams">Базовые параметры.</param>
        /// <param name="taskStatus">Статус задачи.</param>
        public void SendStatus(BaseParams baseParams, TaskStatus taskStatus)
        {
            var httpReferer = baseParams.Params.GetAs<string>("http_referer", ignoreCase: true);
            var reportId = baseParams.Params.GetAs<long>("reportId", ignoreCase: true);
            if (string.IsNullOrWhiteSpace(httpReferer) || reportId == default(long))
            {
                return;
            }
            
            var fileId = baseParams.Params.GetAs<long>("fileId", ignoreCase: true);

            var url = new Uri(new Uri(httpReferer), "TaskStatus/UpdateStatus");
            var runParams = new Dictionary<string, object>
            {
                {"reportId", reportId },
                {"taskStatus", taskStatus },
                {"fileId", fileId }
            };

            var data = JsonConvert.SerializeObject(runParams, Formatting.Indented);
            var dataBytes = Encoding.UTF8.GetBytes(data);

            var request = WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = dataBytes.Length;
            request.Timeout = int.MaxValue;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(dataBytes, 0, data.Length);
            }

            request.GetResponse();
        }
    }
}