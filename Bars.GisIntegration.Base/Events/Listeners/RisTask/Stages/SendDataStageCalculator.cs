namespace Bars.GisIntegration.Base.Events.Listeners.RisTask.Stages
{
    using System;

    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Enums;

    /// <summary>
    /// Калькулятора статусов задачи для этапа выполнения подзадачи отправки данных
    /// </summary>
    public class SendDataStageCalculator : IStageCalculator
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
                return TaskState.SendingData;
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
                    return TaskState.CompleteSuccess;
                case TriggerState.CompleteWithErrors:
                    return TaskState.CompleteWithErrors;
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