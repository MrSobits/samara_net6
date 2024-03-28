namespace Bars.Gkh.ExecutionAction.ExecutionActionScheduler
{
    using System.Collections.Generic;

    using Bars.Gkh.Quartz.Scheduler;
    /// <summary>
    /// Интерфейс планировщика выполняемых действий
    /// </summary>
    public interface IExecutionActionScheduler :  IGkhQuartzScheduler
    {
        /// <summary>
        /// Код для регистрации
        /// </summary>
        string Code { get; }

        /// <summary>
        /// Получить информацию об очереди задач
        /// </summary>
        IList<IExecutionActionJobInfo> GetJobsInfo();
    }
}