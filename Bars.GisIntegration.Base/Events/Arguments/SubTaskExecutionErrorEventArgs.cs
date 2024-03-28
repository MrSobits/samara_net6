namespace Bars.GisIntegration.Base.Events.Arguments
{
    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Аргументы события ошибки выполнения подзадачи
    /// </summary>
    public class SubTaskExecutionErrorEventArgs: SubTaskEventArgs
    {
        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Кнструктор аргументов события ошибки выполнения подзадачи
        /// </summary>
        /// <param name="taskTrigger">Связка триггера и задачи</param>
        /// <param name="message">Сообщение об ошибке</param>
        public SubTaskExecutionErrorEventArgs(RisTaskTrigger taskTrigger, string message)
            : base(taskTrigger)
        {
            this.Message = message;
        }
    }
}
