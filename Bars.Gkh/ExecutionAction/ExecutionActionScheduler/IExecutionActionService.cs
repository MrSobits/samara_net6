namespace Bars.Gkh.ExecutionAction.ExecutionActionScheduler
{
    using Bars.B4;
    using Bars.Gkh.Entities.Administration.ExecutionAction;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Интерфейс работы с выполняемыми действиями
    /// </summary>
    public interface IExecutionActionService
    {
        /// <summary>
        /// Создать задачу для выполнения действия
        /// </summary>
        IDataResult CreateTaskAction(ExecutionActionTask task);

        /// <summary>
        /// Пересоздать задачу для выполнения действия
        /// <para>Запущенная задача будет отменена</para>
        /// </summary>
        IDataResult ReplaceTaskAction(ExecutionActionTask task);

        /// <summary>
        /// Удалить задачу для выполнения действия
        /// </summary>
        IDataResult DeleteTaskAction(ExecutionActionTask task);

        /// <summary>
        /// Получить информацию об очереди планировщика
        /// </summary>
        IDataResult GetSchedulerQueue();

        /// <summary>
        /// Восстановить запланированные задачи
        /// <para>Проставляет незавершенным задачам статус <see cref="ExecutionActionStatus.AbortedOnRestart"/></para>
        /// <para>Запускает обязательные действия при необходимости</para>
        /// </summary>
        void RestoreJobs();

        /// <summary>
        /// Создать задачу из действия
        /// </summary>
        IDataResult CreateTaskFromExecutionAction(string actionCode, BaseParams actionParams = null);
    }
}