namespace Bars.GisIntegration.UI.ViewModel.Task
{
    using System.Collections.Generic;

    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Описатель выполнения задачи
    /// </summary>
    public class TaskExecutionDescriptor
    {
        public TaskExecutionDescriptor(RisTask task, List<TriggerExecutionDescriptor> triggerExecutionDescriptors, byte percent)
        {
            this.Task = task;
            this.TriggerExecutionDescriptors = triggerExecutionDescriptors;
            this.Percent = percent;
        }

        /// <summary>
        /// Задача
        /// </summary>
        public RisTask Task { get; }

        /// <summary>
        /// Описатели выполнения триггеров
        /// </summary>
        public List<TriggerExecutionDescriptor> TriggerExecutionDescriptors { get; }

        /// <summary>
        /// Процент выполнения
        /// </summary>
        public byte Percent { get; }
    }
}
