namespace Bars.Gkh.ExecutionAction.ExecutionActionScheduler.Impl
{
    using System;

    /// <summary>
    /// Ошибка при выполнении задачи
    /// </summary>
    [Serializable]
    public class ExecutionActionJobError
    {
        /// <inheritdoc />
        public string Message { get; set; } = string.Empty;

        /// <inheritdoc />
        public string StackTrace { get; set; } = string.Empty;

        public string Detail { get; set; } = string.Empty;

        /// <inheritdoc />
        public ExecutionActionJobError(Exception exception = null)
        {
            if (exception != null)
            {
                this.Message = exception.InnerException != null
                    ? (exception.InnerException.InnerException?.Message ?? exception.InnerException.Message)
                    : exception.Message;

                this.StackTrace = exception.InnerException != null
                    ? (exception.InnerException.InnerException?.StackTrace ?? exception.InnerException.StackTrace)
                    : exception.StackTrace;

                var npgsqlException = (exception.InnerException?.InnerException) as Npgsql.PostgresException;
                if (npgsqlException != null)
                {
                    this.Detail = npgsqlException.Detail;
                }
            }
        }
    }
}