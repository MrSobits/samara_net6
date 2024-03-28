namespace Bars.GisIntegration.Base.Listeners
{
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Events;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Events;
    using Bars.GisIntegration.Base.Events.Arguments;
    using Bars.Gkh.Quartz.Scheduler.Entities;

    using Castle.Windsor;

    using Quartz;

    using TriggerState = Bars.GisIntegration.Base.Enums.TriggerState;

    /// <summary>
    /// Базовый listener подзадачи
    /// </summary>
    public partial class SubTaskListener : ITriggerListener, IJobListener
    {
        /// <summary>
        /// IoC контейнер
        /// </summary>
        private readonly IWindsorContainer container;

        /// <summary>
        /// Агрегатор событий
        /// </summary>
        private readonly IEventAggregator eventAggregator;

        /// <summary>
        /// Калькулятор состояний подзадачи
        /// </summary>
        private readonly SubTaskStateCalculator subTaskStateCalculator;

        /// <summary>
        /// Задача
        /// </summary>
        private RisTask task;

        /// <summary>
        /// Тип триггера
        /// </summary>
        private TriggerType triggerType;

        /// <summary>
        /// Конструктор базового listener-а
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        /// <param name="task">Задача</param>
        /// <param name="triggerType">Тип триггера</param>
        public SubTaskListener(IWindsorContainer container, RisTask task, TriggerType triggerType)
        {
            this.container = container;
            this.eventAggregator = this.container.Resolve<IEventAggregator>();
            this.subTaskStateCalculator = new SubTaskStateCalculator(this.container);
            this.task = task;
            this.triggerType = triggerType;
        }

        /// <summary>
        /// Имя слушателя
        /// </summary>
        // в случае правок внести аналогичные изменения в SmevSendDataTask.CheckAndAddTriggerListener()
        public string Name => this.GetType().Name + this.task.Id;

        /// <summary>
        /// Начало выполнения подзадачи
        /// </summary>
        /// <param name="taskTrigger">Связка хранимого триггера и задачи</param>
        /// <param name="trigger">Объект триггера</param>
        /// <param name="context">Контекст выполнения задачи</param>
        private void SubTaskStarted(RisTaskTrigger taskTrigger, ITrigger trigger, IJobExecutionContext context)
        {
            this.UpdateState(taskTrigger, TriggerState.Processing);
            this.eventAggregator.GetEvent<SubTaskStartExecutionEvent>().Publish(new SubTaskEventArgs(taskTrigger));
        }

        /// <summary>
        /// Завершение выполения подзадачи
        /// </summary>
        /// <param name="taskTrigger">Связка хранимого триггера и задачи</param>
        /// <param name="trigger">Объект триггера</param>
        /// <param name="context">Контекст выполнения задачи</param>
        /// <param name="triggerInstructionCode">Инструкция планировщика</param>
        private void SubTaskEnd(RisTaskTrigger taskTrigger, ITrigger trigger, IJobExecutionContext context, SchedulerInstruction triggerInstructionCode)
        {
            string message;

            var newState = this.subTaskStateCalculator.CalculateSubTaskEndState(taskTrigger, out message);

            this.UpdateState(taskTrigger, newState, message);

            if (newState == TriggerState.Error)
            {
                this.eventAggregator.GetEvent<SubTaskExecutionErrorEvent>().Publish(new SubTaskExecutionErrorEventArgs(taskTrigger, message));
            }
            else
            {
                this.eventAggregator.GetEvent<SubTaskEndExecutionEvent>().Publish(new SubTaskEventArgs(taskTrigger));
            }       
        }

        /// <summary>
        /// Получить связку триггера и задачи
        /// </summary>
        /// <param name="triggerId">Идентификатор хранимого триггера</param>
        /// <returns>Триггеры, связанные с задачей</returns>
        private RisTaskTrigger GetRisTaskTrigger(long triggerId)
         {
             var taskTriggerRepository = this.container.ResolveDomain<RisTaskTrigger>();

             try
             {
                 return taskTriggerRepository.GetAll().Single(x => x.Trigger.Id == triggerId);
             }
             finally
             {
                 this.container.Release(taskTriggerRepository);
             }
         }

        private RisTaskTrigger SetTaskTriggerRelation(RisTask task, Trigger trigger, TriggerType triggerType)
        {
            var taskTriggerRepository = this.container.ResolveDomain<RisTaskTrigger>();

            try
            {
                var result = new RisTaskTrigger { Task = task, Trigger = trigger, TriggerType = triggerType, TriggerState = TriggerState.Waiting };

                taskTriggerRepository.Save(result);

                return result;
            }
            finally
            {
                this.container.Release(taskTriggerRepository);
            }
        }

        private void UpdateState(RisTaskTrigger taskTrigger, TriggerState newstate, string message = null)
        {
            var taskTriggerRepository = this.container.ResolveDomain<RisTaskTrigger>();

            try
            {
                taskTrigger.TriggerState = newstate;
                taskTrigger.Message = message;

                taskTriggerRepository.Update(taskTrigger);
            }
            finally
            {
                this.container.Release(taskTriggerRepository);
            }
        }
    }
}