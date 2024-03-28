namespace Bars.Gkh.RegOperator.Tasks.Debtors
{
    using Bars.B4;
    using Bars.B4.Modules.Tasks.Common.Contracts;
    using Bars.B4.Modules.Tasks.Common.Contracts.Result;
    using Bars.B4.Modules.Tasks.Common.Service;

    /// <summary>
    /// Провайдер для задачи обновления статусов неплательщиков
    /// </summary>
    public class DebtorsStateTaskProvider : ITaskProvider
    {
        /// <inheritdoc />
        public static string Code = "DebtorsStateUpdate";

        /// <inheritdoc />
        public CreateTasksResult CreateTasks(BaseParams @params)
        {
            return new CreateTasksResult(
                new[]
                {
                    new TaskDescriptor("Обновление статусов неплательщиков", DebtorsStateTaskExecutor.Id, @params)
                    {
                        Dependencies = new[] {new Dependency{Scope = DependencyScope.InsideGlobalTasks, Key = this.TaskCode}}
                    }
                });
        }

        /// <inheritdoc />
        public string TaskCode => DebtorsStateTaskProvider.Code;
    }
}
