namespace Bars.Gkh.ExecutionAction.ExecutionActionScheduler.Impl
{
    using System;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.IoC;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities.Administration.ExecutionAction;
    using Bars.Gkh.Quartz.Job;

    using Castle.MicroKernel.Lifestyle;
    using Castle.Windsor;

    using global::Quartz;

    /// <summary>
    /// Задача обертка для <see cref="IExecutionAction"/>
    /// </summary>
    public sealed class ExecutionActionJob : BaseGkhQuartzJob
    {
        /// <summary>
        /// Ключ для идентификатора задачи <see cref="ExecutionActionTask"/>.Id
        /// </summary>
        public const string TaskIdKey = "ExecutionActionTaskKey";

        /// <summary>
        /// Ключ для идентификатора задачи <see cref="ExecutionActionResult"/>.Id
        /// </summary>
        public const string ResultIdKey = "ExecutionActionResultKey";

        /// <summary>
        /// Ключ для возвращаемого значения
        /// </summary>
        /// <remarks>
        /// <see cref="BaseGkhQuartzJob.GetJobDataMap"/>
        /// <see cref="IJobExecutionContext.Put"/>, <see cref="IJobExecutionContext.Get"/>
        /// </remarks>
        public const string ReturnValueKey = "ExecutionActionReturnValueKey";

        private IWindsorContainer Container => ApplicationContext.Current.Container;

        private string actionCode;

        private IUserIdentity userIdentity;

        private BaseParams executionParams;

        /// <summary>
        /// .ctor
        /// </summary>
        public ExecutionActionJob(ISchedulableTask schedulableTask, string actionCode, BaseParams executionParams, IUserIdentity identity)
        {
            ArgumentChecker.NotNull(schedulableTask, "Не указаны параметры запуска задачи", nameof(schedulableTask));
            ArgumentChecker.NotNullOrEmpty(actionCode, "Не указан код действия", nameof(actionCode));
            ArgumentChecker.NotNull(executionParams, "Указаны недопустимые параметры действия", nameof(executionParams));
            ArgumentChecker.NotNull(identity, "Указаны недопустимый идентификатор пользователя", nameof(identity));

            this.TaskId = schedulableTask.Id;

            this.SchedulableTask = schedulableTask;
            this.actionCode = actionCode;
            this.executionParams = executionParams;
            this.userIdentity = identity;

            this.JobKey = new JobKey(this.TaskId.ToString(), this.actionCode);
            this.TriggerKey = new TriggerKey($"{this.actionCode}_{this.SchedulableTask.Id}");
        }

        /// <inheritdoc />
        public override JobDataMap GetJobDataMap()
        {
            return new JobDataMap
            {
                {ExecutionActionJob.TaskIdKey, this.TaskId},
                {nameof(this.actionCode), this.actionCode},
                {nameof(this.executionParams), this.executionParams },
                {nameof(this.userIdentity), this.userIdentity}
            };
        }

        #region Исполняется планировщиком
        /// <summary>
        /// Конструктор по умолчанию. Необходим для запуска задачи в <see cref="IScheduler"/>
        /// <para>Вызывается неявно</para>
        /// </summary>
        [Obsolete("Use ExecutionActionJob(ISchedulableTask schedulableTask, string actionCode, BaseParams executionParams, IUserIdentity identity)")]
        public ExecutionActionJob()
        {
            this.CreateCancellationTokenSource();
        }

        private void Initialize(IJobExecutionContext context)
        {
            this.JobKey = context.JobDetail.Key;
            this.TriggerKey = context.Trigger.Key;

            this.TaskId = (long)context.JobDetail.JobDataMap.Get(ExecutionActionJob.TaskIdKey);
            this.actionCode = (string)context.JobDetail.JobDataMap.Get(nameof(this.actionCode));
            this.executionParams = (BaseParams)context.JobDetail.JobDataMap.Get(nameof(this.executionParams));
            this.userIdentity = (IUserIdentity)context.JobDetail.JobDataMap.Get(nameof(this.userIdentity));
        }

        /// <inheritdoc />
        public override void Execute(IJobExecutionContext context)
        {
            this.CancellationToken.ThrowIfCancellationRequested();

            this.Initialize(context);

            ExecutionActionJobSyncContext.WaitOrStartJob();

            using (this.Container.BeginScope())
            {
               this.Container.UsingForResolved<IExecutionAction>(this.actionCode,
                    (ioc, action) =>
                    {
                        if (!ExecutionActionJob.IsNeedAction(action as IMandatoryExecutionAction))
                        {
                            context.Put(ExecutionActionJob.ReturnValueKey, BaseDataResult.Error("Действие не требуется"));
                            return;
                        }
                        action.User = this.Container.Resolve<IGkhUserManager>().GetActiveUser();
                        action.ExecutionParams = this.executionParams;
                        action.CancellationToken = this.CancellationToken;

                        var actionResult = action.Action();
                        context.Put(ExecutionActionJob.ReturnValueKey, actionResult);

                        if (actionResult.Success)
                        {
                            action.StartAfterSuccessActions();
                        }
                    });
            }
        }

        private static bool IsNeedAction(IMandatoryExecutionAction mandatoryAction)
        {
            if (mandatoryAction == null)
            {
                return true;
            }

            return mandatoryAction.IsNeedAction();
        }
        #endregion
    }
}