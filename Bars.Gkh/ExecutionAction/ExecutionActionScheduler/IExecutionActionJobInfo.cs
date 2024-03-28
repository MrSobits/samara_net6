namespace Bars.Gkh.ExecutionAction.ExecutionActionScheduler
{
    using Bars.Gkh.Quartz.Job;

    /// <summary>
    /// Информация о задаче
    /// </summary>
    public interface IExecutionActionJobInfo : IGkhQuartzJobInfo
    {
        /// <summary>
        /// Идентификатор задачи <see cref="ISchedulableTask.Id"/>
        /// </summary>
        long TaskId { get; set; }
    }
}