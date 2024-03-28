namespace Bars.Gkh.RegOperator.Tasks.Debtors
{
    using Bars.B4;
    using Bars.B4.Modules.Tasks.Common.Contracts;
    using Bars.B4.Modules.Tasks.Common.Contracts.Result;
    using Bars.B4.Modules.Tasks.Common.Service;

    /// <summary>
    /// Провайдер для задачи создания и обновления ПИР
    /// </summary>
    public class DebtorClaimWorkTaskProvider : ITaskProvider
    {
        /// <inheritdoc />
        public static string Code = "DebtorClaimWorkCreate";

        /// <inheritdoc />
        public CreateTasksResult CreateTasks(BaseParams baseParams)
        {
            return new CreateTasksResult(
                new[]
                {
                    new TaskDescriptor("Создание и обновление ПИР", DebtorClaimWorkTaskExecutor.Id, baseParams)
                    {
                        Dependencies = new[] {new Dependency{Scope = DependencyScope.InsideGlobalTasks, Key = this.TaskCode}},
                        SuccessCallback = DebtorClaimWorkTaskSuccessCallback.Id
                    }
                });
        }

        /// <inheritdoc />
        public string TaskCode => DebtorClaimWorkTaskProvider.Code;
    }
}
