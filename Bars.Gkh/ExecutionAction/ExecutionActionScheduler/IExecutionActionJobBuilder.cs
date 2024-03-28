namespace Bars.Gkh.ExecutionAction.ExecutionActionScheduler
{
    using Bars.B4;
    using Bars.Gkh.ExecutionAction.ExecutionActionScheduler.Impl;
    using Bars.Gkh.Quartz.Job;

    /// <summary>
    /// Фабрика задач
    /// </summary>
    public interface IExecutionActionJobBuilder
    {
        /// <summary>
        /// Установить параметры запуска задачи
        /// </summary>
        IExecutionActionJobBuilder SetSchedulableTask(ISchedulableTask schedulableTask);

        /// <summary>
        /// Установить код действия
        /// </summary>
        IExecutionActionJobBuilder SetActionCode(string actionCode);

        /// <summary>
        /// Установить параметры
        /// </summary>
        /// <param name="baseParams">Параметры для выполнения действия</param>
        IExecutionActionJobBuilder SetParams(BaseParams baseParams);

        /// <summary>
        /// Установить входные параметры
        /// </summary>
        IExecutionActionJobBuilder SetUserIdentity(IUserIdentity userIdentity);

        /// <summary>
        /// Создать <see cref="ExecutionActionJob"/>
        /// </summary>
        IGkhQuartzJob Build();
    }
}