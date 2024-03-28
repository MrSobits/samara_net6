namespace Bars.GisIntegration.Base.Events.Listeners.RisTask.Stages
{
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Enums;

    /// <summary>
    /// Интерфейс калькулятора статусов задачи для этапа выполнения подзадачи
    /// </summary>
    public interface IStageCalculator
    {
        /// <summary>
        /// Получить статус при начале выполнения подзадачи
        /// </summary>
        /// <param name="trigger">Связка триггера и задачи</param>
        /// <returns>Статус задачи</returns>
        TaskState StageStart(RisTaskTrigger trigger);

        /// <summary>
        /// Получить статус при окончании выполнения подзадачи
        /// </summary>
        /// <param name="trigger">Связка триггера и задачи</param>
        /// <returns>Статус задачи</returns>
        TaskState StageEnd(RisTaskTrigger trigger);

        /// <summary>
        /// Получить статус при ошибке выполнения подзадачи
        /// </summary>
        /// <param name="trigger">Связка триггера и задачи</param>
        /// <returns>Статус задачи</returns>
        TaskState StageError(RisTaskTrigger trigger);
    }
}