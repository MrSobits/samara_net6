namespace Bars.Gkh.RegOperator.Tasks.Debtors
{
    using B4;
    using B4.Modules.Tasks.Common.Contracts;
    using B4.Modules.Tasks.Common.Contracts.Result;
    using B4.Modules.Tasks.Common.Service;

    public class DebtorsTaskProvider : ITaskProvider
    {
        public static string Code = "DebtorsCreation";

        #region Implementation of ITaskProvider

        public CreateTasksResult CreateTasks(BaseParams @params)
        {
            return new CreateTasksResult(
                new[]
                {
                    new TaskDescriptor("Формирование неплательщиков", DebtorsTaskExecutor.Id, @params)
                    {
                        Dependencies = new[] {new Dependency{Scope = DependencyScope.InsideGlobalTasks, Key = TaskCode}},
                        SuccessCallback = DebtorTaskSuccessCallback.Id
                    }
                });
        }

        public string TaskCode => DebtorsTaskProvider.Code;
        #endregion
    }
}