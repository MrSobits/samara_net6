namespace Bars.Gkh.Quartz.Scheduler.Extensions
{
    using global::Quartz;

    /// <summary>
    /// Класс с методами, расширяющими контекст выполнения задачи
    /// </summary>
    public static class JobExecutionContext
    {
        /// <summary>
        /// Получить значение
        /// </summary>
        /// <param name="context">Контекст выполнения задачи</param>
        /// <param name="propertyName">Имя свойства</param>
        /// <returns>Значение свойства</returns>
        public static object GetPropertyValue(this IJobExecutionContext context, string propertyName)
        {
            object result;

            if (!context.MergedJobDataMap.TryGetValue(propertyName, out result))
            {
                throw new JobExecutionException(string.Format("Required parameter {0} not found in MergedJobDataMap", propertyName));
            }

            return result;
        }
    }
}
