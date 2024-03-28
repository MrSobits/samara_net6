namespace Bars.Gkh.RegOperator.Tasks.Distribution
{
    using System.Threading;

    using Bars.B4;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.Gkh.RegOperator.Distribution;

    using ExecutionContext = Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    /// <summary>
    /// Задача отмены подтверждения банковской выписки
    /// </summary>
    public class DistributionUndoTaskExecutor : ITaskExecutor
    {
        public IDistributionProvider DistributionProvider { get; set; }

        public static string Id => nameof(DistributionUndoTaskExecutor);

        /// <inheritdoc />
        public IDataResult Execute(BaseParams @params, ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            return this.DistributionProvider.Undo(@params);
        }

        /// <inheritdoc />
        public string ExecutorCode => DistributionUndoTaskExecutor.Id;
    }
}