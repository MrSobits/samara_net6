namespace Bars.GisIntegration.Base.Events.Listeners.RisTask.Stages
{
    using System;

    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Enums;

    /// <summary>
    /// Калькулятора статусов задачи для этапа выполнения подзадачи подготовки данных
    /// </summary>
    public class PrepareDataStageCalculator : IStageCalculator
    {
        /// <summary>
        /// Получить статус при начале выполнения подзадачи
        /// </summary>
        /// <param name="trigger">Связка триггера и задачи</param>
        /// <returns>Статус задачи</returns>
        public TaskState StageStart(RisTaskTrigger trigger)
        {
            if (trigger.TriggerState == TriggerState.Processing)
            {
                return TaskState.PreparingData;
            }

            throw new ArgumentOutOfRangeException();
        }

        /// <summary>
        /// Получить статус при окончании выполнения подзадачи
        /// </summary>
        /// <param name="trigger">Связка триггера и задачи</param>
        /// <returns>Статус задачи</returns>
        public TaskState StageEnd(RisTaskTrigger trigger)
        {
            switch (trigger.TriggerState)
            {
                case TriggerState.CompleteSuccess:
                case TriggerState.CompleteWithErrors:
                    return TaskState.SendDataWaiting;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Получить статус при ошибке выполнения подзадачи
        /// </summary>
        /// <param name="trigger">Связка триггера и задачи</param>
        /// <returns>Статус задачи</returns>
        public TaskState StageError(RisTaskTrigger trigger)
        {
            if (trigger.TriggerState == TriggerState.Error)
            {
                return TaskState.Error;
            }

            throw new ArgumentOutOfRangeException();
        }
    }
}