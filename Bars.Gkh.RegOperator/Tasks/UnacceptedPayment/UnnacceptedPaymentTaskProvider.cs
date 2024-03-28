namespace Bars.Gkh.RegOperator.Tasks.UnacceptedPayment
{
    using B4;
    using B4.Modules.Tasks.Common.Contracts;
    using B4.Modules.Tasks.Common.Contracts.Result;
    using B4.Modules.Tasks.Common.Service;

    public class UnnacceptedPaymentTaskProvider : ITaskProvider
    {
        #region Implementation of ITaskProvider

        public CreateTasksResult CreateTasks(BaseParams @params)
        {
            return new CreateTasksResult(new[] { new TaskDescriptor("Подтверждение неподтвержденных оплат", UnacceptedPaymentTaskExecutor.Id, @params) });
        }

        public string TaskCode { get { return "UnacceptedPaymentAccept"; }}

        #endregion
    }
}