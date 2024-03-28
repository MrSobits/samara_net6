namespace Bars.Gkh.RegOperator.Tasks.Rounding
{
    using B4;    using B4.Modules.Tasks.Common.Contracts;    using B4.Modules.Tasks.Common.Contracts.Result;    using B4.Modules.Tasks.Common.Service;

    class RoundingValuesTaskProvider : ITaskProvider
    {
        public CreateTasksResult CreateTasks(BaseParams @params)        {            return new CreateTasksResult(new[] { new TaskDescriptor("Округление значений начислений в ЛС до 2х знаков", RoundingValuesTasks.Id, @params) });        }        public string TaskCode => "RoundingValues";
    }
}
