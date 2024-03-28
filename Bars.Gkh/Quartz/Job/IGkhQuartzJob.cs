namespace Bars.Gkh.Quartz.Job
{
    using global::Quartz;

    /// <summary>
    /// Описание Quartz.<see cref="IJob"/> задачи
    /// </summary>
    public interface IGkhQuartzJob// : IInterruptableJob TODO quartz
    {
        /// <summary>
        /// Идентификатор задачи
        /// </summary>
        long TaskId { get; set; }

        /// <summary>
        /// Ключ задачи
        /// </summary>
        JobKey JobKey { get; }

        /// <summary>
        /// Ключ триггера
        /// </summary>
        TriggerKey TriggerKey { get; }

        /// <summary>
        /// Получить сущность задачи
        /// </summary>
        IJobDetail GetJob();

        /// <summary>
        /// Получить триггер запуска задачи
        /// </summary>
        ITrigger GetTrigger();
    }
}