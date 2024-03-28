namespace Bars.Gkh.FormatDataExport.Scheduler.Impl
{
    using System;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Utils;
    using Bars.Gkh.ConfigSections.Administration;
    using Bars.Gkh.Entities.Administration.FormatDataExport;
    using Bars.Gkh.FormatDataExport.Tasks;
    using Bars.Gkh.Quartz.Job;
    using Bars.Gkh.Utils;

    using Castle.MicroKernel.Lifestyle;
    using Castle.Windsor;

    using global::Quartz;

    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Экземпляр задачи
    /// </summary>
    public sealed class FormatDataExportJobInstance : BaseGkhQuartzJob
    {
        private IWindsorContainer container;
        private IUserIdentity userIdentity;
        private string name;
        private string groupName;

        /// <summary>
        /// Конструктор по умолчанию для создания объекта планировщиком
        /// </summary>
        [Obsolete("Use FormatDataExportJobInstance(FormatDataExportTask task)")]
        public FormatDataExportJobInstance()
        {
            this.CreateCancellationTokenSource();
        }

        /// <param name="task">Описатель задачи</param>
        public FormatDataExportJobInstance(FormatDataExportTask task)
        {
            this.SchedulableTask = task;
            this.container = ApplicationContext.Current.Container;

            var identity = this.container.Resolve<IUserIdentity>();
            this.userIdentity = identity.IsAuthenticated
                ? identity
                : new UserIdentity(task.User.Id,
                    task.User.Name,
                    Guid.NewGuid().ToString("N"),
                    DynamicDictionary.Create(),
                    "FormatDataExportJobInstance"
                );
            this.name = task.EntityGroupCodeList.IsEmpty()
                ? "ALL"
                : task.EntityGroupCodeList.AggregateWithSeparator(";");
            this.groupName = task.User.Login;

            this.FillInfo();
        }

        private void Init(IJobExecutionContext context)
        {
            this.container = (IWindsorContainer)context.JobDetail.JobDataMap.Get(nameof(this.container));
            this.userIdentity = (IUserIdentity)context.JobDetail.JobDataMap.Get(nameof(this.userIdentity));
            this.name = (string)context.JobDetail.JobDataMap.Get(nameof(this.name));
            this.groupName = (string)context.JobDetail.JobDataMap.Get(nameof(this.groupName));
            this.SchedulableTask = (FormatDataExportTask)context.JobDetail.JobDataMap.Get(nameof(this.SchedulableTask));

            this.FillInfo();
        }

        private void FillInfo()
        {
            var guid = Guid.NewGuid().ToString("N");
            
            this.JobKey = new JobKey(this.name + guid, this.groupName);
            this.TriggerKey = new TriggerKey(this.name + guid, this.groupName);
        }

        /// <inheritdoc />
        public override JobDataMap GetJobDataMap()
        {
            return new JobDataMap
            {
                { nameof(this.container), this.container },
                { nameof(this.userIdentity), this.userIdentity },
                { nameof(this.name), this.name },
                { nameof(this.groupName), this.groupName },
                { nameof(this.SchedulableTask), this.SchedulableTask }
            };
        }

        /// <inheritdoc />
        public override void Execute(IJobExecutionContext context)
        {
            this.Init(context);

            var logManager = this.container.Resolve<ILogger>();
            try
            {
                logManager.LogDebug($"Запуск задачи '{this.JobKey}'");
                this.InternalExecute();
            }
            catch (OperationCanceledException)
            {
                logManager.LogDebug($"Задача '{this.JobKey}' прервана пользователем");
            }
            finally
            {
                logManager.LogDebug($"Завершение задачи '{this.JobKey}'");

                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }

        private void InternalExecute()
        {
            var config = this.container.GetGkhConfig<AdministrationConfig>()
                .FormatDataExport
                .FormatDataExportGeneral;
            
            using (this.container.BeginScope(this.userIdentity))
            {
                if (config.StartInExecutor)
                {
                    this.RunInExecutor();
                }
                else
                {
                    this.Run();
                }
            }
        }

        private void Run()
        {
            var job = this.container.Resolve<FormatDataExportJob>();
            try
            {
                job.ExportTask = this.SchedulableTask as FormatDataExportTask;
                job.CancellationToken = this.CancellationToken;

                job.Execute();
            }
            catch(Exception e)
            {
                this.container.Resolve<ILogger>().LogError(e, "Экспорт данных системы в РИС ЖКХ");
            }
            finally
            {
                this.container.Release(job);
                job = null;
            }
        }

        private void RunInExecutor()
        {
            var taskManager = this.container.Resolve<ITaskManager>();

            var baseParams = new BaseParams { Params = { ["FormatDataExportTaskId"] = this.SchedulableTask.Id } };
            taskManager.CreateTasks(new FormatDataExportTaskProvider(), baseParams);
        }

        /// <inheritdoc />
        public override void Interrupt()
        {
            base.Interrupt();

            var config = this.container.GetGkhConfig<AdministrationConfig>()
                .FormatDataExport
                .FormatDataExportGeneral;
            
            using (this.container.BeginScope(this.userIdentity))
            {
                if (config.StartInExecutor)
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}