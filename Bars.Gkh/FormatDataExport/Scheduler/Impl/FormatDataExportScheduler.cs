namespace Bars.Gkh.FormatDataExport.Scheduler.Impl
{
    using System.Threading;

    using Bars.Gkh.Quartz.Scheduler;

    using global::Quartz;
    using global::Quartz.Impl;
    using global::Quartz.Simpl;

    /// <summary>
    /// Планировщик экспорта данных по формату.
    /// </summary>
    public class FormatDataExportScheduler : BaseGkhQuartzScheduler, IFormatDataExportScheduler
    {
        /// <summary>
        /// Код для регистрации
        /// </summary>
        public static string Code => nameof(FormatDataExportScheduler);

        /// <inheritdoc />
        string IFormatDataExportScheduler.Code => FormatDataExportScheduler.Code;

        private const int DefaultThreadCount = 4;
        private const ThreadPriority DefaultThreadPriority = ThreadPriority.Normal;

        public FormatDataExportScheduler()
        {
            // TODO quartz
            // DirectSchedulerFactory.Instance.CreateScheduler(
            //     FormatDataExportScheduler.Code,
            //     this.GetType().FullName,
            //     new SimpleThreadPool(FormatDataExportScheduler.DefaultThreadCount, FormatDataExportScheduler.DefaultThreadPriority),
            //     new RAMJobStore());
            //
            // this.Scheduler = DirectSchedulerFactory.Instance.GetScheduler(FormatDataExportScheduler.Code).GetResultWithoutContext();
            // this.Scheduler.Start();
        }
    }
}