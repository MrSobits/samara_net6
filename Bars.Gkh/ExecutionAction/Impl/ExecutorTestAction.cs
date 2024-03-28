using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.Tasks.ExecutorTest;
using System;

namespace Bars.Gkh.ExecutionAction.Impl
{
    public class ExecutorTestAction : BaseExecutionAction
    {
        public override string Description => "Ставит тестовую задачу на сервер расчетов";

        public override string Name => "Тест связи с сервером расчетов";

        public override Func<IDataResult> Action => ExecutorTest;

        private IDataResult ExecutorTest()
        {
            var taskManager = Container.Resolve<ITaskManager>();

            try
            {
                taskManager.CreateTasks(new ExecutorTestTaskProvider(), new BaseParams());
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