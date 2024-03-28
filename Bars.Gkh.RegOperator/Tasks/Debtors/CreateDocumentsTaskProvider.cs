namespace Bars.Gkh.RegOperator.Tasks.Debtors
{
    using Bars.B4;
    using Bars.B4.Modules.Tasks.Common.Contracts;
    using Bars.B4.Modules.Tasks.Common.Contracts.Result;
    using Bars.B4.Modules.Tasks.Common.Service;

    /// <summary>
    /// Провайдер для задачи создания документов ПИР
    /// </summary>
    public class CreateDocumentsTaskProvider : ITaskProvider
    {
        /// <inheritdoc />
        public static string Code = "DebtorCreateDocuments";

        /// <inheritdoc />
        public CreateTasksResult CreateTasks(BaseParams baseParams)
        {
            return new CreateTasksResult(
                new[]
                {
                    new TaskDescriptor("Автоматическое создание документов ПИР", CreateDocumentsTaskExecutor.Id, baseParams)
                    {
                        Dependencies = new[] {new Dependency{Scope = DependencyScope.InsideGlobalTasks, Key = this.TaskCode}}
                    }
                });
        }

        /// <inheritdoc />
        public string TaskCode => CreateDocumentsTaskProvider.Code;
    }
}
