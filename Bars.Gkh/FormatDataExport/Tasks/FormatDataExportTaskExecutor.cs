namespace Bars.Gkh.FormatDataExport.Tasks
{
    using System;
    using System.Threading;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities.Administration.FormatDataExport;
    using Bars.Gkh.FormatDataExport.Scheduler.Impl;

    using Castle.Windsor;

    using ExecutionContext = Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    /// <summary>
    /// Задачи на экспорт в формате 4.0
    /// </summary>
    public class FormatDataExportTaskExecutor : ITaskExecutor
    {
        /// <summary>
        /// Код задачи
        /// </summary>
        public static string Id = "FormatDataExportTask";

        /// <inheritdoc />
        public string ExecutorCode => FormatDataExportTaskExecutor.Id;

        public IWindsorContainer Container { get; set; }
        public IRepository<FormatDataExportTask> FormatDataExportTaskRepository { get; set; }

        /// <inheritdoc />
        public IDataResult Execute(BaseParams baseParams, ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            var job = this.Container.Resolve<FormatDataExportJob>();
            try
            {
                var taskId = baseParams.Params.GetAsId("FormatDataExportTaskId");
                job.ExportTask = this.FormatDataExportTaskRepository.Get(taskId);

                job.CancellationToken = ct;
                job.ProgressIndicator = indicator;
                job.Execute();

                return new BaseDataResult();
            }
            finally
            {
                this.Container.Release(job);
                job = null;

                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }
    }
}