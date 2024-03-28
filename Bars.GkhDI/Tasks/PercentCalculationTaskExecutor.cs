namespace Bars.GkhDi.Tasks
{
    using System.Threading;

    using Bars.B4;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.GkhDi.DomainService;

    using ExecutionContext = Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    /// <summary>
    /// Задача по расчёту раскрытия информации
    /// </summary>
    public class PercentCalculationTaskExecutor : ITaskExecutor
    {
        /// <summary>
        /// Сервис раскрытия информации
        /// </summary>
        public IDisclosureInfoService Service { get; set; }

        /// <inheritdoc />
        public IDataResult Execute(BaseParams baseParams, ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            return this.Service.StartPercentCalculation(baseParams);
        }

        /// <inheritdoc />
        public string ExecutorCode => nameof(PercentCalculationTaskExecutor);
    }
}