namespace Bars.Gkh.Quartz.Scheduler
{
    using Bars.B4;
    using Bars.Gkh.Quartz.Job;

    /// <summary>
    /// Интерфейс Quartz планировщика
    /// </summary>
    public interface IGkhQuartzScheduler
    {
        /// <summary>
        /// Запланировать действие
        /// </summary>
        IDataResult ScheduleJob(IGkhQuartzJob job);

        /// <summary>
        /// Отменить запланированное действие
        /// </summary>
        IDataResult UnScheduleJob(IGkhQuartzJob job);

        /// <summary>
        /// Получить информацию об очереди задач
        /// </summary>
        string GetJobQueue();
    }
}