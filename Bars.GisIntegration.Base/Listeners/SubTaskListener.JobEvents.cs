namespace Bars.GisIntegration.Base.Listeners
{
    using Quartz;

    /// <summary>
    /// Базовый listener подзадачи
    /// Обработчики событий job
    /// </summary>
    public partial class SubTaskListener
    {
        /// <summary>
        /// Метод обработки события, когда задача должна быть выполнена (сработал ассоциированный триггер).
        /// </summary>
        /// <param name="context">Контекст выполнения задачи</param>
        public void JobToBeExecuted(IJobExecutionContext context)
        {
        }

        /// <summary>
        /// Метод обработки события, когда выполнение задачи отменено слушателем триггера
        /// </summary>
        /// <param name="context">Контекст выполнения задачи</param>
        public void JobExecutionVetoed(IJobExecutionContext context)
        {
        }

        /// <summary>
        /// Метод обработки события после запуска выполнения задачи
        /// </summary>
        /// <param name="context">Контекст выполнения задачи</param>
        /// <param name="jobException">Исключение запуска выполнения задачи</param>
        public void JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException)
        {
        }
    }
}
