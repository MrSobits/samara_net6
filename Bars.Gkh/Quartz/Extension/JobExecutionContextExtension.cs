namespace Bars.Gkh.Quartz.Extension
{
    using global::Quartz;

    public static class JobExecutionContextExtension
    {
        /// <summary>
        /// Получить значение из <see cref="JobDataMap"/> по ключу
        /// </summary>
        public static T GetValue<T>(this IJobExecutionContext context, string key, T defaultValue = default (T))
        {
            var result = context.JobDetail.JobDataMap.Get(key);

            if (result == null)
            {
                return defaultValue;
            }

            if (result is T)
            {
                return (T) context.JobDetail.JobDataMap.Get(key);
            }

            return defaultValue;
        }

        /// <summary>
        /// Получить значение по ключу в <see cref="JobDataMap"/>
        /// </summary>
        public static void PutValue(this IJobExecutionContext context, string key, object value)
        {
            context.JobDetail.JobDataMap.Put(key, value);
        }
    }
}