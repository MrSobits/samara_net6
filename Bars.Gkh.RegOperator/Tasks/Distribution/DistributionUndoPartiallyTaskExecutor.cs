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
    public class DistributionUndoPartiallyTaskExecutor : ITaskExecutor
    {
        public IDistributionProvider DistributionProvider { get; set; }

        public static string Id => nameof(DistributionUndoPartiallyTaskExecutor);

        /// <inheritdoc />
        public IDataResult Execute(BaseParams @params, ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            return this.DistributionProvider.UndoPartially(@params);
        }

        /// <inheritdoc />
        public string ExecutorCode => DistributionUndoPartiallyTaskExecutor.Id;
    }
}