namespace Bars.Gkh.ExecutionAction.ExecutionActionScheduler.Impl
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Bars.B4;
    using Bars.Gkh.Quartz.Extension;

    using global::Quartz;

    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Обработчик задач
    /// </summary>
    public class ExecutionActionJobListener : IJobListener
    {
        /// <summary>
        /// Код для регистрации
        /// </summary>
        public static string Code => typeof(ExecutionActionJobListener).GUID.ToString();

        /// <summary>
        /// Get the name of the <see cref="T:Quartz.IJobListener"/>.
        /// </summary>
        public string Name => ExecutionActionJobListener.Code;

        /// <summary>
        /// Логгер
        /// </summary>
        public ILogger LogManager {get; set; }

        /// <summary>
        /// Интерфейс сохранения статуса задачи
        /// <para>
        /// Получить идентификатор задачи можно по ключу <see cref="IExecutionActionJobStateReporter.JobIdKeyName"/> в
        /// коллекции <see cref="IJobDetail"/>.JobDetail.JobDataMap
        /// </para>
        /// </summary>
        private readonly IExecutionActionJobStateReporter jobStateReporter;

        /// <summary>
        /// .ctor
        /// </summary>
        public ExecutionActionJobListener(IExecutionActionJobStateReporter jobStateReporter)
        {
            this.jobStateReporter = jobStateReporter;
        }

        /// <summary>
        /// Задача запущена
        /// </summary>
        /// <seealso cref="M:Quartz.IJobListener.JobExecutionVetoed(Quartz.IJobExecutionContext)"/>
        public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            var taskId = context.GetValue<long>(ExecutionActionJob.TaskIdKey);
            var resultId = this.jobStateReporter.JobStarted(taskId);

            context.PutValue(ExecutionActionJob.ResultIdKey, resultId);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Выполнение задания было запрещено триггером.
        /// <para>Подробнее: <see cref="ITriggerListener.VetoJobExecution"/></para>
        /// </summary>
        /// <seealso cref="M:Quartz.IJobListener.JobToBeExecuted(Quartz.IJobExecutionContext)"/>
        public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new System.NotImplementedException("Задача прервана триггером");
        }

        /// <summary>
        /// Задача завершена
        /// </summary>
        public Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                var jobResult = this.GetJobResult(context);
                var result = jobException == null
                    ? new ExecutionActionJobResult(jobResult)
                    : new ExecutionActionJobResult(jobException);

                var resultId = context.GetValue<long>(ExecutionActionJob.ResultIdKey);

                this.jobStateReporter.JobEnded(resultId, result);
            }
            catch (Exception exception)
            {
               this.LogManager.LogError(exception, exception.Message);
            }

            return Task.CompletedTask;
        }

        private IDataResult GetJobResult(IJobExecutionContext context)
        {
            return context.Get(ExecutionActionJob.ReturnValueKey) as IDataResult;
        }
    }
}