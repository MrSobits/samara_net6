namespace Bars.Gkh.Quartz.Scheduler
{
    using System;
    using System.Globalization;
    using System.Reflection;

    using Bars.B4.IoC;

    using Castle.Windsor;

    using global::Quartz;
    using global::Quartz.Simpl;
    using global::Quartz.Spi;

    using NLog;

    /// <summary>
    /// Фабрика задач
    /// </summary>
    public class TaskFactory : PropertySettingJobFactory
    {
        /// <summary>
        /// Объект логировщика
        /// </summary>
        private Logger logger = LogManager.GetLogger(typeof(TaskFactory).FullName);

        /// <summary>
        /// IoC контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Метод создания нового экземпляра исполнителя задачи
        /// </summary>
        /// <param name="bundle">Объект - связка сработки триггера</param>
        /// <param name="scheduler">Экземпляр планировщика</param>
        /// <returns>Экземпляр исполнителя</returns>
        public override IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            IJobDetail jobDetail = bundle.JobDetail;
            Type taskType = jobDetail.JobType;
            IJob result = null;

            if (taskType == null)
            {
                throw new ArgumentNullException("bundle.JobDetail.JobType", "Cannot instantiate null");
            }

            try
            {
                var taskName = taskType.Name;

                if (this.logger.IsDebugEnabled)
                {
                    this.logger.Debug(string.Format(CultureInfo.InvariantCulture, "Producing instance of Task '{0}', class={1}", jobDetail.Key, taskType.FullName));
                }

                if (this.Container.Kernel.HasComponent(taskName))
                {
                    result = this.Container.Resolve<IJob>(taskName);
                }
                else
                {
                    ConstructorInfo constructorInfo = taskType.GetConstructor(Type.EmptyTypes);

                    if (constructorInfo == null)
                    {
                        throw new ArgumentException("Cannot instantiate type which has no empty constructor", taskName);
                    }

                    result = (IJob)constructorInfo.Invoke(new object[0]);
                }
            }
            catch (Exception exception)
            {
                var schedulerException = new SchedulerException(string.Format(CultureInfo.InvariantCulture, "Problem instantiating class '{0}'", taskType.FullName), exception);
                throw schedulerException;
            }

            JobDataMap jobDataMap = new JobDataMap();
            jobDataMap.PutAll(scheduler.Context);
            jobDataMap.PutAll(bundle.JobDetail.JobDataMap);
            jobDataMap.PutAll(bundle.Trigger.JobDataMap);

            this.SetObjectProperties(result, jobDataMap);

            return result;
        }

        /// <summary>
        /// Очистка созданной задачи
        /// </summary>
        public override void ReturnJob(IJob job)
        {
            this.Container.TearDown(job);
            base.ReturnJob(job);
        }
    }
}
