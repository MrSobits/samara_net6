namespace Bars.B4.Modules.Analytics.Reports.Tasks
{
    using System;
    using System.Threading;

    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Analytics.Reports.Domain;
    using Bars.B4.Modules.Analytics.Reports.Entities;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Tasks.Common.Contracts;
    using Bars.B4.Modules.Tasks.Common.Contracts.Result;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Utils;

    using ExecutionContext = Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    /// <summary>
    /// Задача генерации отчёта
    /// </summary>
    public class ReportGeneratorTask : ITaskProvider, ITaskExecutor
    {
        private string reportName;

        /// <summary>
        /// Ключ регистрации
        /// </summary>
        public static string Id = nameof(ReportGeneratorTask);

        /// <inheritdoc />
        public string TaskCode => ReportGeneratorTask.Id;

        /// <inheritdoc />
        public string ExecutorCode => ReportGeneratorTask.Id;

        /// <summary>
        /// Интерфейс сервиса генерации отчётов
        /// </summary>
        public IReportGeneratorService ReportGeneratorService { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="StoredReport"/>
        /// </summary>
        public IDomainService<StoredReport> StoredReportDomain { get; set; }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="reportName">Наименование отчёта</param>
        public ReportGeneratorTask(string reportName) : this()
        {
            this.reportName = reportName;
        }

        /// <summary>
        /// .ctor
        /// </summary>
        public ReportGeneratorTask()
        {
        }

        /// <inheritdoc />
        public CreateTasksResult CreateTasks(BaseParams baseParams)
        {
            return new CreateTasksResult(new[]
            {
                new TaskDescriptor("Формирование отчёта", this.ExecutorCode, baseParams)
                {
                    Description = this.reportName,
                    Dependencies = new[]
                    {
                        new Dependency
                        {
                            Key = this.ExecutorCode,
                            Scope = DependencyScope.InsideGlobalTasks
                        }, 
                    }
                } 
            });
        }

        /// <inheritdoc />
        public IDataResult Execute(BaseParams baseParams, ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            var reportId = baseParams.Params.GetAs<long>("reportId", ignoreCase: true);
            var report = this.StoredReportDomain.FirstOrDefault(x => x.Id == reportId);
            var format = baseParams.Params.GetAs("format", ignoreCase: true, defaultValue: ReportPrintFormat.xls);

            var result = this.ReportGeneratorService.SaveOnServer(baseParams, report, format);

            return new BaseDataResult(result.Data.Id);
        }
    }
}