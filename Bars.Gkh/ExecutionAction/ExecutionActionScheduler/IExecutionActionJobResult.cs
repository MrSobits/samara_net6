namespace Bars.Gkh.ExecutionAction.ExecutionActionScheduler
{
    using Bars.B4;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Результат выполнения действия
    /// </summary>
    public interface IExecutionActionJobResult
    {
        /// <summary>
        /// Статус действия
        /// </summary>
        ExecutionActionStatus Status { get; }

        /// <summary>
        /// Результат
        /// </summary>
        IDataResult Data { get; }
    }
}