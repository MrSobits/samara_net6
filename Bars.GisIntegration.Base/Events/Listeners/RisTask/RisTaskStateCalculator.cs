namespace Bars.GisIntegration.Base.Events.Listeners.RisTask
{
    using System;

    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.Events;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Events.Arguments;
    using Bars.GisIntegration.Base.Events.Listeners.RisTask.Stages;

    using Castle.Windsor;

    /// <summary>
    /// Калькулятор состояний задачи
    /// </summary>
    public class RisTaskStateCalculator
    {
        /// <summary>
        /// Конструктор калькулятора состояний задачи
        /// </summary>
        /// <param name="task">Задача</param>
        /// <param name="calculator">Калькулятор статусов задачи для этапа выполнения подзадачи</param>
        public RisTaskStateCalculator(RisTask task, IStageCalculator calculator)
        {
            this.Container = ApplicationContext.Current.Container;
            this.EventAggregator = ApplicationContext.Current.Events;

            this.RisTaskId = task.Id;
            this.Calculator = calculator;

            Func<RisTaskTrigger, bool> filter = trigger => trigger.Task.Id == this.RisTaskId;

            this.EventAggregator.GetEvent<SubTaskStartExecutionEvent>()
                .Subscribe(this.ProcessStartEvent, ThreadOption.PublisherThread, true, args => filter(args.TaskTrigger));
            this.EventAggregator.GetEvent<SubTaskEndExecutionEvent>().Subscribe(this.ProcessEndEvent, ThreadOption.PublisherThread, true, args => filter(args.TaskTrigger));
            this.EventAggregator.GetEvent<SubTaskExecutionErrorEvent>().Subscribe(this.ProcessErrorEvent, ThreadOption.PublisherThread, true, args => filter(args.TaskTrigger));
        }

        private IWindsorContainer Container { get; }

        private IEventAggregator EventAggregator { get; }

        private long RisTaskId { get; }

        private IStageCalculator Calculator { get; }

        /// <summary>
        /// Обрабатывать этапы подзадачи подготовки данных
        /// </summary>
        /// <param name="task">Задача</param>
        public static void HandlePrepareDataStage(RisTask task)
        {
            new RisTaskStateCalculator(task, new PrepareDataStageCalculator());
        }

        /// <summary>
        /// Обрабатывать этапы подзадачи отправки данных
        /// </summary>
        /// <param name="task">Задача</param>
        public static void HandleSendDataStage(RisTask task)
        {
            new RisTaskStateCalculator(task, new SendDataStageCalculator());
        }

        private void ProcessStartEvent(SubTaskEventArgs args)
        {
            this.SetState(args.TaskTrigger.Task, this.Calculator.StageStart(args.TaskTrigger));
        }

        private void ProcessEndEvent(SubTaskEventArgs args)
        {
            this.SetState(args.TaskTrigger.Task, this.Calculator.StageEnd(args.TaskTrigger));

            this.Unsubscribe();
        }

        private void ProcessErrorEvent(SubTaskExecutionErrorEventArgs args)
        {
            this.SetState(args.TaskTrigger.Task, this.Calculator.StageError(args.TaskTrigger), args.Message);

            this.Unsubscribe();
        }

        private void Unsubscribe()
        {
            this.EventAggregator.GetEvent<SubTaskStartExecutionEvent>().Unsubscribe(this.ProcessStartEvent);
            this.EventAggregator.GetEvent<SubTaskEndExecutionEvent>().Unsubscribe(this.ProcessEndEvent);
            this.EventAggregator.GetEvent<SubTaskExecutionErrorEvent>().Unsubscribe(this.ProcessErrorEvent);
        }

        private void SetState(RisTask risTask, TaskState newState, string message = null)
        {
            var domain = this.Container.ResolveDomain<RisTask>();
            try
            {
                risTask.TaskState = newState;
                risTask.Message = message;

                if (newState == TaskState.PreparingData)
                {
                    risTask.StartTime = DateTime.Now;
                }

                if (newState == TaskState.CompleteSuccess || newState == TaskState.CompleteWithErrors || newState == TaskState.Error)
                {
                    risTask.EndTime = DateTime.Now;
                }

                domain.Update(risTask);
            }
            finally
            {
                this.Container.Release(domain);
            }
        }
    }
}