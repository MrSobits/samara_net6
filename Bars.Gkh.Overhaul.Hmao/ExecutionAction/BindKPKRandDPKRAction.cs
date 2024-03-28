using System;
using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.ExecutionAction;
using Bars.Gkh.Overhaul.Hmao.Task;

namespace Bars.Gkh.Overhaul.Hmao.ExecutionAction
{
    public class BindKPKRandDPKRAction : BaseExecutionAction
    {
        public override string Name => "Сопоставить записи КПКР и ДПКР";

        public override string Description => "Проставляет связку между ДПКР и КПКР по адресу, году и КЭ";

        public override Func<IDataResult> Action => Execute;

        private BaseDataResult Execute()
        {
            var taskManager = Container.Resolve<ITaskManager>();

            try
            {
                taskManager.CreateTasks(new BindKPKRandDPKRTaskProvider(), ExecutionParams);
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
