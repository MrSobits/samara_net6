using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.ExecutionAction;
using Bars.Gkh.Regions.Voronezh.Tasks;
using Bars.Gkh.Tasks.ExecutorTest;
using System;

namespace Bars.Gkh.Regions.Voronezh.ExecutionAction.Impl
{
    public class SetSSPPaymentsAction : BaseExecutionAction
    {
        public override string Description => "ПИР - Задача по автозаполнению графы Погашено в рамках исполнительного производства";

        public override string Name => "ПИР - Заполнение оплат в рамках исполнительного производства";

        public override Func<IDataResult> Action => GetPaymentsSSP;

        private IDataResult GetPaymentsSSP()
        {
            var taskManager = Container.Resolve<ITaskManager>();

            try
            {
                taskManager.CreateTasks(new SetSSPPaymentsProvider(), new BaseParams());
                return new BaseDataResult(true, "Задача успешно поставлена");
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, e.Message);
            }
            finally
            {
                Container.Release(taskManager);
            }
        }
    }
}

