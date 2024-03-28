namespace Bars.Gkh.FormatDataExport.Scheduler.Impl
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.Events;
    using Bars.B4.IoC;
    using Bars.Gkh.Entities.Administration.FormatDataExport;
    using Bars.Gkh.Enums;

    using Castle.MicroKernel.Lifestyle;
    using Castle.Windsor;

    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Сервис постановки задач экспорта в планировщик
    /// </summary>
    public class FormatDataExportSchedulerService : EventHandlerBase<AppStartEventArgs>, IFormatDataExportSchedulerService
    {
        public IWindsorContainer Container { get; set; }
        public IFormatDataExportScheduler Scheduler { get; set; }
        public ILogger LogManager { get; set; }

        /// <inheritdoc />
        public void Init()
        {
            this.CheckErrorJobs();

            this.ScheduleStoredJobs();
        }

        /// <inheritdoc />
        public IDataResult ScheduleJob(FormatDataExportJobInstance job)
        {
            return this.Scheduler.ScheduleJob(job);
        }

        /// <inheritdoc />
        public IDataResult UnScheduleJob(FormatDataExportJobInstance job)
        {
            return this.Scheduler.UnScheduleJob(job);
        }

        /// <inheritdoc />
        public override void OnEvent(AppStartEventArgs args)
        {
            ApplicationContext.Current.Container.BuildUp(this);

            try
            {
                using (this.Container.BeginScope())
                {
                    this.Init();
                }
                
                this.LogManager.LogDebug(this.Scheduler.GetJobQueue());
            }
            catch (Exception e)
            {
                this.LogManager.LogError(e, "Ошибка при инициализиции планировщика экспорта данных по формату");
            }
        }

        private void ScheduleStoredJobs()
        {
            this.Container.UsingForResolved<IDomainService<FormatDataExportTask>>((container, domain) =>
            {
                var activeJobs = domain.GetAll()
                    .Where(x => !x.IsDelete)
                    .Where(x => x.PeriodType != TaskPeriodType.NoPeriodicity)
                    .Where(x => !x.StartDate.HasValue || x.StartDate < DateTime.Today)
                    .Where(x => !x.EndDate.HasValue || x.EndDate > DateTime.Today);

                foreach (var dataExportTask in activeJobs)
                {
                    this.ScheduleJob(new FormatDataExportJobInstance(dataExportTask));
                }
            });
        }

        private void CheckErrorJobs()
        {
            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                var results = this.Container.ResolveDomain<FormatDataExportResult>();
                try
                {
                    var jobResult = results.GetAll()
                        .Where(x => x.Status == FormatDataExportStatus.Pending
                            || x.Status == FormatDataExportStatus.Running);

                    foreach (var job in jobResult)
                    {
                        job.Status = FormatDataExportStatus.RuntimeError;
                        job.Progress = 0f;

                        results.Update(job);
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    this.Container.Resolve<ILogger>().LogError(ex, ex.Message);
                }
                finally
                {
                    this.Container.Release(results);
                }
            }
        }
    }
}