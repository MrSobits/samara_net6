namespace Bars.Gkh.Quartz.Scheduler
{
    using System.Text;

    using Bars.B4;
    using Bars.Gkh.ExecutionAction.ExecutionActionScheduler;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.Quartz.Job;

    using Castle.Windsor;

    using global::Quartz;
    using global::Quartz.Impl.Matchers;

    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Базовый класс Quartz планировщика
    /// </summary>
    public abstract class BaseGkhQuartzScheduler : IGkhQuartzScheduler
    {
        protected IScheduler Scheduler;
        protected IExecutionActionJobStateReporter JobStateReporter;

        public IWindsorContainer Container { get; set; }
        public ILogger LogManager { get; set; }

        /// <summary>
        /// Запланировать действие
        /// </summary>
        public virtual IDataResult ScheduleJob(IGkhQuartzJob job)
        {
            var quartzJob = job.GetJob();

            var scheduleTime = this.Scheduler.ScheduleJob(quartzJob, job.GetTrigger());

            return new BaseDataResult(scheduleTime);
        }

        /// <summary>
        /// Отменить запланированное действие
        /// </summary>
        public virtual IDataResult UnScheduleJob(IGkhQuartzJob job)
        {
            var result = new BaseDataResult(job);

            if (this.Scheduler.CheckExists(job.TriggerKey).GetResultWithoutContext())
            {
                result.Success &= this.Scheduler.UnscheduleJob(job.TriggerKey).GetResultWithoutContext();
            }

            if (this.Scheduler.CheckExists(job.JobKey).GetResultWithoutContext())
            {
                result.Success &= this.Scheduler.Interrupt(job.JobKey).GetResultWithoutContext();
            }
            
            return result;
        }

        /// <summary>
        /// Получить информацию об очереди задач
        /// </summary>
        public virtual string GetJobQueue()
        {
            var jobGroups = this.Scheduler.GetJobGroupNames().GetResultWithoutContext();
            var sb = new StringBuilder();
            sb.AppendLine($"[{this.GetType().FullName}]");
            foreach (var group in jobGroups)
            {
                var groupMatcher = GroupMatcher<JobKey>.GroupContains(group);
                var jobKeys = this.Scheduler.GetJobKeys(groupMatcher).GetResultWithoutContext();
                foreach (var jobKey in jobKeys)
                {
                    var detail = this.Scheduler.GetJobDetail(jobKey).GetResultWithoutContext();
                    var triggers = this.Scheduler.GetTriggersOfJob(jobKey).GetResultWithoutContext();
                    foreach (ITrigger trigger in triggers)
                    {
                        sb.AppendFormat($"\tJob group: {group}\r\n");
                        sb.AppendFormat($"\tJob name: {jobKey.Name}\r\n");
                        sb.AppendFormat($"\tJob description: {detail.Description}\r\n");
                        sb.AppendFormat($"\tTrigger name: {trigger.Key.Name}\r\n");
                        sb.AppendFormat($"\tTrigger group: {trigger.Key.Group}\r\n");
                        sb.AppendFormat($"\tTriggerType name: {trigger.GetType().Name}\r\n");
                        sb.AppendFormat($"\tTrigger state: {this.Scheduler.GetTriggerState(trigger.Key)}\r\n");
                        var nextFireTime = trigger.GetNextFireTimeUtc();
                        if (nextFireTime.HasValue)
                        {
                            sb.AppendFormat($"\tNext fire time: {nextFireTime.Value.LocalDateTime}\r\n");
                        }

                        var previousFireTime = trigger.GetPreviousFireTimeUtc();
                        if (previousFireTime.HasValue)
                        {
                            sb.AppendFormat($"\tPrevious fire time: {previousFireTime.Value.LocalDateTime}\r\n");
                        }

                        sb.AppendLine();
                    }
                }
            }
            return sb.ToString();
        }
    }
}