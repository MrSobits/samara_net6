namespace Bars.GisIntegration.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Enums;
    using Bars.Gkh.Quartz.Scheduler;
    using Bars.Gkh.Quartz.Scheduler.Entities;

    using Castle.Windsor;

    using Quartz;

    using TriggerState = Bars.GisIntegration.Base.Enums.TriggerState;

    /// <summary>
    /// Калькулятор состояний подзадачи
    /// </summary>
    public class SubTaskStateCalculator
    {
        /// <summary>
        /// Конструктор калькулятора состояний подзадачи
        /// </summary>
        /// <param name="сontainer">IoC контейнер</param>
        public SubTaskStateCalculator(IWindsorContainer сontainer)
        {
            this.Container = сontainer;
            this.TaskManager = this.Container.Resolve<ITaskManager>();
            this.Scheduler = this.Container.Resolve<IScheduler>("TaskScheduler");
        }

        /// <summary>
        /// IoC контейнер
        /// </summary>
        public IWindsorContainer Container { get; }

        /// <summary>
        /// Менеджер задач
        /// </summary>
        public ITaskManager TaskManager { get; }

        /// <summary>
        /// Планировщик задач
        /// </summary>
        public IScheduler Scheduler { get; }

        /// <summary>
        /// Рассчитать состояние завершения подзадачи
        /// </summary>
        /// <param name="taskTrigger">Связка триггера и задачи</param>
        /// <param name="message">Сообщение об ошибке</param>
        /// <returns>Итоговое состояние</returns>
        public TriggerState CalculateSubTaskEndState(RisTaskTrigger taskTrigger, out string message)
        {
            var triggerJournal = this.GetTriggerJournal(taskTrigger.Trigger.Id);

            if (this.TriggerExecutionError(triggerJournal, out message))
            {
                return TriggerState.Error;
            }

            if (this.TriggerCompleteWithErrors(taskTrigger, triggerJournal))
            {
                return TriggerState.CompleteWithErrors;
            }

            return TriggerState.CompleteSuccess;
        }

        public byte GetExecutionPercent(RisTaskTrigger taskTrigger)
        {
            byte result = 0;

            switch (taskTrigger.TriggerState)
            {
                case TriggerState.Error:
                case TriggerState.CompleteSuccess:
                case TriggerState.CompleteWithErrors:
                    result = 100;
                    break;
                case TriggerState.Processing:
                    if (taskTrigger.TriggerType == TriggerType.PreparingData)
                    {
                        result = this.GetPreparingDataPercent(taskTrigger.Trigger.QuartzTriggerKey);
                    }
                    else if (taskTrigger.TriggerType == TriggerType.SendingData)
                    {
                        result = this.GetSendingDataPercent(taskTrigger.Trigger);
                    }
                    break;
            }

            return result;
        }

        private byte GetPreparingDataPercent(string quartzTriggerKey)
        {
            var currentlyExecutingTriggers = this.Scheduler.GetCurrentlyExecutingJobs();

            var currentlyExecutingContext = currentlyExecutingTriggers.FirstOrDefault(x => x.Trigger.Key.Name == quartzTriggerKey);

            if (currentlyExecutingContext != null)
            {
                var taskInstance = currentlyExecutingContext.JobInstance as ITask;

                return taskInstance?.Percent ?? 0;
            }

            return 0;
        }

        private byte GetSendingDataPercent(Trigger trigger)
        {
            var packages = this.TaskManager.GetTriggerPackages(trigger.Id).ToList();

            var totalCount = packages.Count;

            var processedCount =
                packages.Count(
                    x =>
                    x.State == PackageState.SendingError || x.State == PackageState.SuccessProcessed || x.State == PackageState.ProcessedWithErrors || x.State == PackageState.GettingStateError
                    || x.State == PackageState.ProcessingResultError || x.State == PackageState.TimeoutError);

            if (totalCount == processedCount)
            {
                return 100;
            }

            if (processedCount == 0)
            {
                return 0;
            }

            return (byte)(100 * processedCount / totalCount);
        }

        /// <summary>
        /// Получить журнал выполнения триггера
        /// </summary>
        /// <param name="triggerId">Идентификатор триггера</param>
        /// <returns>Список записей журнала выполнения триггера</returns>
        private List<JournalRecord> GetTriggerJournal(long triggerId)
        {
            var journalRepository = this.Container.ResolveDomain<JournalRecord>();

            try
            {
                return journalRepository.GetAll().Where(x => x.Trigger.Id == triggerId).ToList();
            }
            finally
            {
                this.Container.Release(journalRepository);
            }
        }

        private bool TriggerExecutionError(IList<JournalRecord> triggerJournal, out string message)
        {
            message = null;

            var result = false;

            if (triggerJournal.Count > 0)
            {
                var errorRecord = triggerJournal.FirstOrDefault(x => !string.IsNullOrEmpty(x.Message));

                if (errorRecord != null)
                {
                    message = errorRecord.Message;
                    result = true;
                }
            }

            return result;
        }

        private bool TriggerCompleteWithErrors(RisTaskTrigger taskTrigger, IList<JournalRecord> triggerJournal)
        {
            switch (taskTrigger.TriggerType)
            {
                case TriggerType.PreparingData:
                    return this.PreparingDataCompleteWithErrors(triggerJournal);
                case TriggerType.SendingData:
                    return this.SendingDataCompleteWithErrors(taskTrigger.Trigger);
                default:
                    return false;
            }
        }

        private bool PreparingDataCompleteWithErrors(IList<JournalRecord> triggerJournal)
        {
            //триггер подготовки данных планируется на 1 запуск
            if (triggerJournal.Count == 1)
            {
                var prepareDataResult = this.TaskManager.GetSerializablePreparingDataTriggerResult(triggerJournal[0]);
                return prepareDataResult == null || prepareDataResult.PackageIds.Count == 0 || prepareDataResult.ValidateResult.Count > 0;
            }

            throw new Exception("Не корректное количество записей в журнале триггера подготовки данных");
        }

        private bool SendingDataCompleteWithErrors(Trigger trigger)
        {
            var triggerPackages = this.TaskManager.GetTriggerPackages(trigger.Id);

            return triggerPackages.Any(x => x.State != PackageState.SuccessProcessed);
        }
    }
}
