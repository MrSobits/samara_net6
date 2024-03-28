namespace Bars.Gkh.RegOperator.Tasks.UnacceptedPayment
{
    using B4;
    using B4.Modules.Tasks.Common.Contracts;
    using B4.Modules.Tasks.Common.Contracts.Result;
    using B4.Modules.Tasks.Common.Service;

    public class MassDebtStartDateCalculateTaskProvider : ITaskProvider
    {
        public CreateTasksResult CreateTasks(BaseParams @params)
        {
            return new CreateTasksResult(new[] { new TaskDescriptor("Расчёт эталонных оплат", MassDebtStartDateCalculateTaskExecutor.Id, @params) });
        }

        public string TaskCode { get { return "MassDebtStartDateCalculate"; }}
    }
}