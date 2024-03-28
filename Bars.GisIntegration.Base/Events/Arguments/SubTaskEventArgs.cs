namespace Bars.GisIntegration.Base.Events.Arguments
{
    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Аргументы события подзадачи
    /// </summary>
    public class SubTaskEventArgs
    {
        /// <summary>
        /// Связка триггера и задачи
        /// </summary>
        public RisTaskTrigger TaskTrigger { get; }

        /// <summary>
        /// Конструктор аргументов события подзадачи
        /// </summary>
        /// <param name="taskTrigger">Связка триггера и задачи</param>
        public SubTaskEventArgs(RisTaskTrigger taskTrigger)
        {
            this.TaskTrigger = taskTrigger;
        }
    }
}
