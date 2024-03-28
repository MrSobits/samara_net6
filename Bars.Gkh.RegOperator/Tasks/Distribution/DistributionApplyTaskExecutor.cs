namespace Bars.Gkh.RegOperator.Tasks.Distribution
{
    using System.Threading;

    using Bars.B4;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.Gkh.RegOperator.Distribution;

    using ExecutionContext = Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    /// <summary>
    /// Задача подтверждения банковских выписок
    /// </summary>
    public class DistributionApplyTaskExecutor : ITaskExecutor
    {
        public IDistributionProvider DistributionProvider { get; set; }

        public static string Id => nameof(DistributionApplyTaskExecutor);

        /// <inheritdoc />
        public IDataResult Execute(BaseParams @params, ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            return this.DistributionProvider.Apply(@params);
        }

        /// <inheritdoc />
        public string ExecutorCode => DistributionApplyTaskExecutor.Id;
    }
}