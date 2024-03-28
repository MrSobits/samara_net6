namespace Bars.GisIntegration.Base.Listeners
{
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Enums;
    using Bars.Gkh.Quartz.Scheduler;
    using Bars.Gkh.Quartz.Scheduler.Entities;
    using Bars.Gkh.Quartz.Scheduler.Extensions;

    using Quartz;

    using TriggerState = Quartz.TriggerState;

    /// <summary>
    /// Базовый listener подзадачи
    /// Обработчики событий trigger
    /// </summary>
    public partial class SubTaskListener
    {
        /// <summary>
        /// Метод обработки события, когда сработал триггер.
        /// </summary>
        /// <param name="trigger">Объект триггера</param>
        /// <param name="context">Контекст выполнения задачи</param>
        public void TriggerFired(ITrigger trigger, IJobExecutionContext context)
        {
            ExecutionOwnerScope.CallInNewScope(
                context,
                () =>
                {
                    var storableTrigger = (Trigger)context.GetPropertyValue("StorableTrigger");
                  
                    if (context.PreviousFireTimeUtc == null)
                    {
                        var taskTrigger = this.SetTaskTriggerRelation(this.task, storableTrigger, this.triggerType);
                        context.MergedJobDataMap.Put("TaskTrigger", taskTrigger);

                        this.SubTaskStarted(taskTrigger, trigger, context);
                    }
                    else
                    {
                        var storableTriggerId = (long)context.GetPropertyValue("storableTriggerId");
                        var taskTrigger = this.GetRisTaskTrigger(storableTriggerId);

                        context.MergedJobDataMap.Put("TaskTrigger", taskTrigger);
                    }
                });
        }

        /// <summary>
        /// Метод обработки события отмены выполнения задачи триггером
        /// </summary>
        /// <param name="trigger">Объект триггера</param>
        /// <param name="context">Контекст выполнения задачи</param>
        /// <returns>true - если выполнение задачи должно быть отменено, false - в противном случае</returns>
        public bool VetoJobExecution(ITrigger trigger, IJobExecutionContext context)
        {
            return false;
        }

        /// <summary>
        /// Метод обработки события, когда произошла осечка триггера
        /// </summary>
        /// <param name="trigger">Объект триггера</param>
        public void TriggerMisfired(ITrigger trigger)
        {
        }

        /// <summary>
        /// Метод обработки события, когда задача выполнена
        /// </summary>
        /// <param name="trigger">Объект триггера</param>
        /// <param name="context">Контекст выполнения задачи</param>
        /// <param name="triggerInstructionCode">Инструкция планировщика</param>
        public void TriggerComplete(ITrigger trigger, IJobExecutionContext context, SchedulerInstruction triggerInstructionCode)
        {
            ExecutionOwnerScope.CallInNewScope(
                context,
                () =>
                {
                    var triggerState = context.Scheduler.GetTriggerState(context.Trigger.Key);

                    if (triggerState == TriggerState.None || triggerState == TriggerState.Complete || !context.Trigger.GetMayFireAgain())
                    {
                        var taskTrigger = (RisTaskTrigger)context.GetPropertyValue("TaskTrigger");
                        this.SubTaskEnd(taskTrigger, trigger, context, triggerInstructionCode);
                    }
                });
        }
    }
}
