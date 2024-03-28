namespace Bars.Gkh.RegOperator.Tasks.Debtors
{
    using System.Reflection;
    using System.Threading;

    using Bars.B4;
    using Bars.B4.Modules.Tasks.Common.Contracts.Result;
    using Bars.B4.Modules.Tasks.Common.Service;

    using Castle.Windsor;

    using ExecutionContext = Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    public class DebtorTaskSuccessCallback : ITaskCallback
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        private readonly IWindsorContainer container;

        public DebtorTaskSuccessCallback(IWindsorContainer container)
        {
            this.container = container;
        }

        public CallbackResult Call(long taskId, BaseParams baseParams, ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            var updateClaimWorks = baseParams.Params.GetAs<bool>("updateClaimWorks");
            if (updateClaimWorks)
            {
                var taskManager = this.container.Resolve<ITaskManager>();
                try
                {
                    taskManager.CreateTasks(new DebtorClaimWorkTaskProvider(), baseParams);
                }
                finally
                {
                    this.container.Release(taskManager);
                }
            }

            return new CallbackResult(true);
        }
    }
}