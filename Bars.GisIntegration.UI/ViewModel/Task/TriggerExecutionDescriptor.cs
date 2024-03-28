namespace Bars.GisIntegration.UI.ViewModel.Task
{
    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Описатель выполнения триггера
    /// </summary>
    public class TriggerExecutionDescriptor
    {
        /// <summary>
        /// Конструктор описателя выполнения триггера
        /// </summary>
        /// <param name="taskTrigger">Связка триггера и задачи</param>
        /// <param name="percent">Процент выполнения</param>
        public TriggerExecutionDescriptor(RisTaskTrigger taskTrigger, byte percent)
        {
            this.TaskTrigger = taskTrigger;
            this.Percent = percent;
        }

        /// <summary>
        /// Связка триггера и задачи
        /// </summary>
        public RisTaskTrigger TaskTrigger { get; }

        /// <summary>
        /// Процент выполнения
        /// </summary>
        public byte Percent { get; }
    }
}
