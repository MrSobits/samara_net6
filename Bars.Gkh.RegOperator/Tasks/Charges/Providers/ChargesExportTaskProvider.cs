namespace Bars.Gkh.RegOperator.Tasks.Charges.Providers
{
    using B4;
    using B4.Modules.Tasks.Common.Contracts;
    using B4.Modules.Tasks.Common.Contracts.Result;
    using B4.Modules.Tasks.Common.Service;
    using Executors;

    public class ChargesExportTaskProvider : ITaskProvider
    {
        #region Implementation of ITaskProvider

        public CreateTasksResult CreateTasks(BaseParams @params)
        {
            return new CreateTasksResult(new[] { new TaskDescriptor("Выгрузка начислений", ChargesExportTaskExecutor.Id, @params) });
        }

        public string TaskCode { get { return "ChargeExport"; }}

        #endregion
    }
}