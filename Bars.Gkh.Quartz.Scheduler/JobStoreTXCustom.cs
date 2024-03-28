namespace Bars.Gkh.Quartz.Scheduler
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;

    using B4.DataAccess;
    using Bars.B4.Application;
    using Bars.Gkh.Quartz.Scheduler.Entities;

    using Castle.Windsor;

    using global::Quartz;
    using global::Quartz.Impl.AdoJobStore;
    using global::Quartz.Impl.Triggers;
    using global::Quartz.Spi;

    /// <summary>
    /// Хранилище задач планировщика
    /// </summary>
    public class JobStoreTXCustom: JobStoreTX
    {
        /// <summary>
        /// IoC контейнер
        /// </summary>
        public IWindsorContainer Container { get; }

        /// <summary>
        /// Конструктор хранилища задач
        /// </summary>
        public JobStoreTXCustom()
        {
            this.Container = ApplicationContext.Current.Container;
        }

        /// <summary>
        /// Insert or update a trigger.
        /// </summary>
        protected override void StoreTrigger(
            ConnectionAndTransactionHolder conn,
            IOperableTrigger newTrigger,
            IJobDetail job,
            bool replaceExisting,
            string state,
            bool forceState,
            bool recovering)
        {
            var executionOwner = this.GetExecutionOwner(newTrigger);

            ExecutionOwnerScope.CallInNewScope(
                executionOwner,
                () =>
                {
                    if (!this.StorableTriggerExists(newTrigger.Key.Name))
                    {
                        var storableTrigger = this.SaveTrigger(newTrigger, job, executionOwner);

                        newTrigger.JobDataMap.Put("storableTriggerId", storableTrigger.Id);
                    }                    
                });

            base.StoreTrigger(conn, newTrigger, job, replaceExisting, state, forceState, recovering);         
        }

        private Trigger SaveTrigger(ITrigger trigger, IJobDetail jobDetail, IExecutionOwner executionOwner)
        {
            var triggerRepository = this.Container.ResolveDomain<Trigger>();

            try
            {
                var storableTrigger = this.CreateStorableTrigger(trigger, jobDetail, executionOwner);
                triggerRepository.Save(storableTrigger);

                return storableTrigger;
            }
            finally
            {
                this.Container.Release(triggerRepository);
            }
        }

        private Trigger CreateStorableTrigger(ITrigger trigger, IJobDetail jobDetail, IExecutionOwner executionOwner)
        {
            Trigger result = null;

            var repeatCount = -1;
            var repeatInterval = 0;

            var simpleTrigger = trigger as SimpleTriggerImpl;

            if (simpleTrigger != null)
            {
                repeatCount = simpleTrigger.RepeatCount;
                repeatInterval = simpleTrigger.RepeatInterval.Seconds;
            }

            var className = jobDetail?.JobType.Name ?? string.Empty;

            result = new Trigger
            {
                ClassName = className,
                QuartzTriggerKey = trigger.Key.Name,
                StartParams = this.GetStartParameters(trigger),
                RepeatCount = repeatCount,
                Interval = repeatInterval,
                UserName = executionOwner != null ? executionOwner.Name : string.Empty
            };

            return result;
        }

        private IExecutionOwner GetExecutionOwner(ITrigger trigger)
        {
            object result;

            if (trigger.JobDataMap.TryGetValue("ExecutionOwner", out result))
            {
                return (IExecutionOwner)result;
            }

            return null;
        }

        private byte[] GetStartParameters(ITrigger trigger)
        {
            using (var memoryStream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(memoryStream, trigger.JobDataMap);

                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Возвращает хранимый триггер по имени
        /// </summary>
        /// <param name="triggerName">Имя триггера</param>
        /// <returns>Хранимый объект триггера</returns>
        private bool StorableTriggerExists(string triggerName)
        {
            var triggerRepository = this.Container.ResolveDomain<Trigger>();

            try
            {
                return triggerRepository.GetAll().Any(x => x.QuartzTriggerKey == triggerName);
            }
            finally
            {
                this.Container.Release(triggerRepository);
            }
        }
    }
}
