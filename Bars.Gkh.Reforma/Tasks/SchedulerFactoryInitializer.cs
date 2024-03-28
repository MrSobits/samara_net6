namespace Bars.Gkh.Reforma.Tasks
{
    using System;
    using System.Collections.Specialized;
    using System.Threading;

    using global::Quartz.Impl;

    /// <summary>
    /// Инициализатор планировщика задач для ручной интеграции с Реформой.ЖКХ
    /// </summary>
    public static class SchedulerFactoryInitializer
    {
        private static int poolSize = 1;
        private static readonly Lazy<StdSchedulerFactory> instance = new Lazy<StdSchedulerFactory>(SchedulerFactoryInitializer.InitSchedulerFactory, LazyThreadSafetyMode.ExecutionAndPublication);
        /// <summary>
        /// Получить сконфигурированный планировщик
        /// </summary>
        /// <returns>Планировщик</returns>
        public static StdSchedulerFactory CreateSchedulerFactory()
        {
            return SchedulerFactoryInitializer.instance.Value;
        }

        private static StdSchedulerFactory InitSchedulerFactory()
        {
            var parameters = new NameValueCollection
            {
                { "quartz.threadPool.threadCount", SchedulerFactoryInitializer.poolSize.ToString() },
                { "quartz.scheduler.instanceName", "ReformaTaskScheduler" },
                { "quartz.scheduler.instanceId", "ReformaTaskScheduler" }
            };

            return new StdSchedulerFactory(parameters);
        }
    }
}