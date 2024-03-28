using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.ExecutionAction;
using Bars.Gkh.Tasks;
using System;

namespace Bars.Gkh.RegOperator.ExecutionAction
{
    public class RecalcSaldoAction : BaseExecutionAction
    {
        public override string Description => "Пересчитываем сальдо по группе";

        public override string Name => "Пересчет сальдо по группе ЛС \"Пересчет сальдо\"";

        public override Func<IDataResult> Action => RecalcSaldo;

        private IDataResult RecalcSaldo()
        {
            var taskManager = Container.Resolve<ITaskManager>();

            try
            {
                taskManager.CreateTasks(new RecalcSaldoProvider(), new BaseParams());
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