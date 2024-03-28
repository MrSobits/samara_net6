namespace Bars.Gkh.RegOperator.Tasks.Period.Callbacks
{
    using System.Reflection;
    using System.Threading;
    using Bars.B4;
    using Bars.B4.Modules.Tasks.Common.Contracts.Result;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.Gkh.RegOperator.Tasks.Period.Providers;
    using ExecutionContext = Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    public class PeriodCloseSuccessCallback_Step2 : ITaskCallback
    {
        private readonly ITaskManager _taskMan;
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public PeriodCloseSuccessCallback_Step2(ITaskManager taskMan)
        {
            _taskMan = taskMan;
        }

        #region Implementation of ITaskCallback

        public CallbackResult Call(long taskId,
            BaseParams @params,
            ExecutionContext ctx,
            IProgressIndicator indicator,
            CancellationToken ct)
        {
            _taskMan.CreateTasks(new PeriodCloseTaskProvider_Step3(), @params, _taskMan.GetParentId(taskId));

            return new CallbackResult(true);
        }

        #endregion
    }
}