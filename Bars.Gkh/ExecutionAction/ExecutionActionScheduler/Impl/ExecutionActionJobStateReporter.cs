namespace Bars.Gkh.ExecutionAction.ExecutionActionScheduler.Impl
{
    using System;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Entities.Administration.ExecutionAction;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Extensions;

    using Castle.Windsor;

    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Сохраняет информацию о статусе задачи в БД
    /// </summary>
    public class ExecutionActionJobStateReporter : IExecutionActionJobStateReporter
    {
        /// <summary>
        /// Код для регистрации
        /// </summary>
        public static string Code => typeof(ExecutionActionJobStateReporter).GUID.ToString();

        /// <summary>
        /// Ключ иденитфикатора задачи
        /// </summary>
        public string JobIdKeyName => ExecutionActionJobStateReporter.Code;

        public ILogger LogManager { get; set; }
        public IWindsorContainer Container { get; set; }
        public IRepository<ExecutionActionTask> ExecutionActionTaskRepository { get; set; }
        public IRepository<ExecutionActionResult> ExecutionActionResultRepository { get; set; }
        public IExecutionActionScheduler ExecutionActionScheduler { get; set; }

        /// <inheritdoc />
        public long JobStarted(long taskId)
        {
            var result = this.Container.InTransactionWithResultInNewScope(() =>
            {
                var task = this.ExecutionActionTaskRepository.Load(taskId);
                ArgumentChecker.NotNull(task, nameof(task));

                var taskResult = new ExecutionActionResult
                {
                    StartDate = DateTime.Now,
                    Task = task,
                    Status = ExecutionActionStatus.Running
                };

                this.ExecutionActionResultRepository.Save(taskResult);

                return new BaseDataResult(taskResult);
            });

            return (result.Data as ExecutionActionResult)?.Id ?? 0;
        }

        /// <inheritdoc />
        public void JobEnded(long tasResultId, IExecutionActionJobResult result)
        {
            this.Container.InTransactionInNewScope(() =>
            {
                var taskResult = this.ExecutionActionResultRepository.Get(tasResultId);

                taskResult.EndDate = DateTime.Now;
                taskResult.Status = result.Status;
                taskResult.Result = result.Data;

                this.ExecutionActionResultRepository.Save(taskResult);
            });

            ExecutionActionJobSyncContext.EndJob();
        }
    }
}