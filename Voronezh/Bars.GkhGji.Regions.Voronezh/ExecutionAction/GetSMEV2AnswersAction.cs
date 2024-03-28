using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.ExecutionAction;
using Bars.GkhGji.Regions.Voronezh.Tasks.GetSMEVAnswers;
using System;

namespace Bars.GkhGji.Regions.Voronezh.ExecutionAction
{
    /// <summary>
    /// Периодическая проверка СМЭВа2 на результаты
    /// </summary>
    public class GetSMEV2AnswersAction : BaseExecutionAction//, IMandatoryExecutionAction
    {
        public override string Description => "Запрашивает из СМЭВа2 все ответы и обрабатывает их";

        public override string Name => "Проверить ответы в СМЭВ2";

        public override Func<IDataResult> Action => GetSMEV2Responses;

       // public bool IsNeedAction() => true;

        private IDataResult GetSMEV2Responses()
        {
            var taskManager = Container.Resolve<ITaskManager>();

            try
            {
                taskManager.CreateTasks(new GetSMEV2AnswersTaskProvider(Container), new BaseParams());
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
