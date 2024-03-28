namespace Bars.Gkh.Quartz.Scheduler
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Quartz.Scheduler.Entities;
    using Bars.Gkh.Quartz.Scheduler.Extensions;
    using Bars.Gkh.Quartz.Scheduler.Log;

    using Castle.Windsor;

    using global::Quartz;

    /// <summary>
    /// Базовая задача
    /// </summary>
    [PersistJobDataAfterExecution]
    [DisallowConcurrentExecution]
    public abstract class BaseTask: ITask
    {
        private readonly ReaderWriterLockSlim lockObject = new ReaderWriterLockSlim();
        private List<ILogRecord> log = new List<ILogRecord>();

        /// <summary>
        /// IoC контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Инициатор выполнения задачи
        /// </summary>
        public IExecutionOwner ExecutionOwner { get; private set; }

        /// <summary>
        /// Процент выполнения работы
        /// </summary>
        public virtual byte Percent { get; protected set; }

        /// <summary>
        /// Запрошено прерывание выполнения задачи
        /// </summary>
        public bool InterruptRequested { get; private set; }

        /// <summary>
        /// Исключение, возникшее при выполнении задачи, если оно было
        /// </summary>
        public Exception ExecutionException { get; protected set; }

        public Trigger StorableTrigger { get; set; }

        /// <summary>
        /// Отменить запланированное выполнение триггера при возникновении ошибки
        /// </summary>
        protected virtual bool UnscheduleOnError => false;

        /// <summary>
        /// Метод выполнения задачи, вызываемый планировщиком при сработке триггера
        /// </summary>
        /// <param name="context">Контекст выполнения задачи</param>
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                this.SetExecutionOwner(context);

                if (this.CheckExecutionAccess(this.ExecutionOwner))
                {
                    ExecutionOwnerScope.CallInNewScope(
                        this.ExecutionOwner,
                        () =>
                        {
                            this.Initialize(context);
                            context.Result = this.DoWork(context);
                        });
                }
                else
                {
                    throw new Exception("Job execution access denied.");
                }
            }
            catch (Exception exception)
            {
                this.ExecutionException = exception;

                if (this.UnscheduleOnError)
                {
                    context.Scheduler.UnscheduleJob(context.Trigger.Key);
                }

                throw new JobExecutionException(exception);
            }
        }

        /// <summary>
        /// Метод, вызываемый планировщиком, для прерывания процесса выполнения задачи
        /// </summary>
        public void Interrupt()
        {
            this.InterruptRequested = true;
        }

        /// <summary>
        /// Возвращает коллекцию записей лога
        /// </summary>
        /// <returns>Список записей лога</returns>
        public List<ILogRecord> GetLog()
        {
            this.lockObject.EnterReadLock();

            try
            {
                return this.log;
            }
            finally
            {
                this.lockObject.ExitReadLock();
            }
        }

        /// <summary>
        /// Проверить доступ к выполнению задачи
        /// </summary>
        /// <param name="executionOwner">Инициатор выполнения</param>
        /// <returns>true - доступ разрешен, false - в противном случае</returns>
        protected virtual bool CheckExecutionAccess(IExecutionOwner executionOwner)
        {
            return true;
        }

        /// <summary>
        /// Метод выполнения задачи. При выполнении проверять значение флага InterruptRequested.
        /// </summary>
        /// <param name="context">Контекст выполнения задачи</param>
        /// <returns>Результат выполнения задачи</returns>
        protected abstract object DoWork(IJobExecutionContext context);

        /// <summary>
        /// Добавление записей лога в коллекцию
        /// </summary>
        /// <param name="logRecords">Новые записи лога</param>
        protected void AddLogRecord(IEnumerable<ILogRecord> logRecords)
        {
            this.lockObject.EnterWriteLock();

            try
            {
                this.log.AddRange(logRecords);
            }
            finally
            {
                this.lockObject.ExitWriteLock();
            }
        }

        /// <summary>
        /// Добавление записи лога в коллекцию
        /// </summary>
        /// <param name="logRecord">Новая запись лога</param>
        protected void AddLogRecord(ILogRecord logRecord)
        {
            this.AddLogRecord(new List<ILogRecord>() { logRecord });
        }

        private void SetExecutionOwner(IJobExecutionContext context)
        {
            try
            {
                this.ExecutionOwner = (IExecutionOwner)context.GetPropertyValue("ExecutionOwner");
            }
            catch (Exception exception)
            {
                throw new JobExecutionException("Obtaining execution owner error.", exception, false);
            }
        }

        private void Initialize(IJobExecutionContext context)
        {
            try
            {
                var storableTriggerId = (long)context.GetPropertyValue("storableTriggerId");

                this.StorableTrigger = this.GetTrigger(storableTriggerId);
            }
            catch (Exception exception)
            {
                throw new JobExecutionException("Obtaining storable trigger error.", exception, false);
            }
        }

        /// <summary>
        /// Получить хранимый триггер
        /// </summary>
        /// <param name="triggerId">Идентификатор триггера</param>
        /// <returns>Триггер</returns>
        private Trigger GetTrigger(long triggerId)
        {
            var triggerRepository = this.Container.ResolveDomain<Trigger>();

            try
            {
                var result = triggerRepository.Get(triggerId);

                if (result != null)
                {
                    return result;
                }

                throw new Exception($"Storable trigger {triggerId} is not found.");
            }
            finally
            {
                this.Container.Release(triggerRepository);
            }
        }
    }
}
