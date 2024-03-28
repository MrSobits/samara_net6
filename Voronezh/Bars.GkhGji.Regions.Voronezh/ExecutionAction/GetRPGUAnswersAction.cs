using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.ExecutionAction;
using Bars.GkhGji.Regions.Voronezh.Tasks.GetSMEVAnswers;
using System;

namespace Bars.GkhGji.Regions.Voronezh.ExecutionAction
{
    /// <summary>
    /// Периодическая проверка регионального СМЭВ на результаты
    /// </summary>
    public class GetRPGUAnswersAction : BaseExecutionAction//, IMandatoryExecutionAction
    {
        public override string Description => "Запрашивает из СГИО все ответы и обрабатывает их";

        public override string Name => "Проверить ответы в СГИО";

        public override Func<IDataResult> Action => GetRPGUResponses;

       // public bool IsNeedAction() => true;

        private IDataResult GetRPGUResponses()
        {
            var taskManager = Container.Resolve<ITaskManager>();

            try
            {
                taskManager.CreateTasks(new GetRPGUAnswersTaskProvider(Container), new BaseParams());
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
