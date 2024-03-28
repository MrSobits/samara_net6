using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.ExecutionAction;
using Bars.GkhGji.Regions.BaseChelyabinsk.Tasks;
using System;

namespace Bars.GkhGji.Regions.BaseChelyabinsk.ExecutionAction
{
    /// <summary>
    /// Периодическая проверка СМЭВа на результаты
    /// </summary>
    public class GetSMEVAction : BaseExecutionAction
    {
        public override string Description => "Запрашивает из СМЭВа все ответы и обрабатывает их (сертификат)";

        public override string Name => "Проверить ответы в СМЭВ (Base)";

        public override Func<IDataResult> Action => GetSMEVResponses;     

        private IDataResult GetSMEVResponses()
        {
            var taskManager = Container.Resolve<ITaskManager>();

            try
            {
                taskManager.CreateTasks(new GetSMEVAnswersProvider(Container), new BaseParams());
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
