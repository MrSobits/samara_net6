namespace Bars.Gkh.ExecutionAction.ExecutionActionScheduler.Impl
{
    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Quartz.Job;

    using Castle.Core;
    using Castle.Windsor;

    /// <summary>
    /// Фабрика задач
    /// </summary>
    public class ExecutionActionJobBuilder : IExecutionActionJobBuilder, IInitializable
    {
        /// <summary>
        /// Код для регистрации
        /// </summary>
        public static string Code => typeof(ExecutionActionJobBuilder).GUID.ToString();

        public IWindsorContainer Container { get; set; }

        private ISchedulableTask executionActionSchedulableTask;
        private string executionActionCode;
        private BaseParams executionParams;
        private IUserIdentity executionActionUserIdentity;

        public IExecutionActionJobBuilder SetSchedulableTask(ISchedulableTask schedulableTask)
        {
            this.executionActionSchedulableTask = schedulableTask;
            return this;
        }

        /// <inheritdoc />
        public IExecutionActionJobBuilder SetActionCode(string actionCode)
        {
            this.executionActionCode = actionCode;
            return this;
        }

        /// <inheritdoc />
        public IExecutionActionJobBuilder SetParams(BaseParams baseParams)
        {
            this.executionParams = baseParams;
            return this;
        }

        /// <inheritdoc />
        public IExecutionActionJobBuilder SetUserIdentity(IUserIdentity userIdentity)
        {
            this.executionActionUserIdentity = userIdentity;
            return this;
        }

        /// <inheritdoc />
        public IGkhQuartzJob Build()
        {
            return new ExecutionActionJob(this.executionActionSchedulableTask,
                this.executionActionCode,
                this.executionParams,
                this.executionActionUserIdentity);
        }

        /// <inheritdoc />
        public void Initialize()
        {
            this.executionActionSchedulableTask = new BaseSchedulableTask
            {
                StartNow = true,
                PeriodType = TaskPeriodType.NoPeriodicity
            };
            this.executionParams = new BaseParams();
        }
    }
}