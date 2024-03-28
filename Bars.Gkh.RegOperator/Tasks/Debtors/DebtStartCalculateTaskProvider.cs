namespace Bars.Gkh.RegOperator.Tasks.Debtors
{
    using Bars.B4;
    using Bars.B4.Modules.Tasks.Common.Contracts;
    using Bars.B4.Modules.Tasks.Common.Contracts.Result;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.Gkh.RegOperator.Domain.Repository.Wallets;

    /// <summary>
    /// Выполняет задачу рассчета даты начала долга и суммы лога
    /// </summary>
    public class DebtStartCalculateTaskProvider : ITaskProvider
    {
        /// <inheritdoc />
        public static string Code = "DebtStartCalculate";

        /// <inheritdoc />
        public CreateTasksResult CreateTasks(BaseParams baseParams)
        {
            var docId = baseParams.Params.GetAs<long>("docId");

            return new CreateTasksResult(
                new[]
                {
                    new TaskDescriptor("Расчет даты начала долга и суммы ПИР", DebtStartCalculateTaskExecutor.Id, baseParams)
                    {
                        Dependencies = new[] {new Dependency{Scope = DependencyScope.InsideGlobalTasks, Key = this.TaskCode + docId.ToString()}}
                    }
                });
        }

        /// <inheritdoc />
        public string TaskCode => CreateDocumentsTaskProvider.Code;
    }
}
