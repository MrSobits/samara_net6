namespace Bars.Gkh.FormatDataExport.Scheduler
{
    using Bars.B4;
    using Bars.Gkh.FormatDataExport.Scheduler.Impl;

    /// <summary>
    /// Сервис постановки задач экспорта в планировщик
    /// </summary>
    public interface IFormatDataExportSchedulerService
    {
        /// <summary>
        /// Инициализировать планировщик при запуске приложения
        /// </summary>
        void Init();

        /// <summary>
        /// Поставить задачу в очередь
        /// </summary>
        IDataResult ScheduleJob(FormatDataExportJobInstance job);

        /// <summary>
        /// Удалить задачу из очереди
        /// </summary>
        IDataResult UnScheduleJob(FormatDataExportJobInstance job);
    }
}