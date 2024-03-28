namespace Bars.Gkh.ExecutionAction.ExecutionActionScheduler
{
    /// <summary>
    /// Сохраняет информацию о статусе задачи
    /// </summary>
    public interface IExecutionActionJobStateReporter
    {
        /// <summary>
        /// Задача запущена
        /// </summary>
        long JobStarted(long taskId);

        /// <summary>
        /// Задача завершена
        /// </summary>
        void JobEnded(long taskResulId, IExecutionActionJobResult result);
    }
}