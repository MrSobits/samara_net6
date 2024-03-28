namespace Bars.Gkh.RegOperator.Tasks.Loans
{
    using B4;
    using B4.Modules.Tasks.Common.Contracts;
    using B4.Modules.Tasks.Common.Contracts.Result;
    using B4.Modules.Tasks.Common.Service;

    public class LoanTakerTaskProvider : ITaskProvider
    {
        public CreateTasksResult CreateTasks(BaseParams baseParams)
        {
            return new CreateTasksResult(new[] { new TaskDescriptor("Автоматическое взятие займа", LoanTakerTaskExecutor.Id, baseParams) });
        }

        public string TaskCode => nameof(LoanTakerTaskProvider);
    }
}