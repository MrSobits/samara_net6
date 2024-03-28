namespace Bars.Gkh.ExecutionAction.ExecutionActionScheduler.Impl
{
    using System;

    using Bars.B4;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Extensions;

    /// <summary>
    /// Обертка результата выполнения действия
    /// </summary>
    public class ExecutionActionJobResult : IExecutionActionJobResult
    {
        /// <inheritdoc />
        public ExecutionActionStatus Status { get; }

        /// <inheritdoc />
        public IDataResult Data { get; }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="result">Результат выполнения действия <see cref="IExecutionAction.Action"/></param>
        public ExecutionActionJobResult(IDataResult result)
        {
            if (result == null)
            {
                this.Status = ExecutionActionStatus.RuntimeError;
                this.Data = BaseDataResult.Error("Действие не вернуло результат");
            }
            else
            {
                this.Status = result.Success ? ExecutionActionStatus.Success : ExecutionActionStatus.Error;
                this.Data = this.SerializeResult(result);
            }
        }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="exception">Исключение при выполнении задачи</param>
        public ExecutionActionJobResult(Exception exception)
        {
            this.Status = exception?.InnerException?.InnerException is OperationCanceledException
                ? ExecutionActionStatus.Cancelled
                : ExecutionActionStatus.RuntimeError;

            this.Data = this.SerializeResult(
                new BaseDataResult(new ExecutionActionJobError(exception))
                {
                    Message = exception.Message + " " + exception.StackTrace.ToString(),
                    Success = false
                });
        }

        private IDataResult SerializeResult(IDataResult result)
        {
            if (result == null)
            {
                return result;
            }

            var dataResult = result.Data.ToJson();
            result.Data = dataResult;

            return result;
        }
    }
}