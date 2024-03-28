using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.Entities;
using Bars.Gkh.ExecutionAction;
using Bars.GkhGji.Regions.Chelyabinsk.Entities;
using Bars.GkhGji.Regions.Chelyabinsk.Tasks.CreateResolutionPayFines;
using System;
using System.Collections.Generic;

using System.Linq;

namespace Bars.GkhGji.Regions.Chelyabinsk.ExecutionAction
{
    /// <summary>
    /// Периодическый запрос оплат в СМЭВ
    /// </summary>
    public class CreateResolutionPayFinesAction : BaseExecutionAction//, IMandatoryExecutionAction
    {
        //сколько запрашивать выписок за раз
        static int numberOfRequests = 15;

        public override string Description => "Взаимодействие постановлений с ГИС ГМП в части отправки начислений, проверки оплат и тд.";

        public override string Name => "Отправить начисления, заполнить оплаты штрафов и пошлин";

        public override Func<IDataResult> Action => CreateResolutionPayFines;

        //public bool IsNeedAction() => true;

        private IDataResult CreateResolutionPayFines()
        {
            var taskManager = Container.Resolve<ITaskManager>();

            try
            {
                taskManager.CreateTasks(new CreateResolutionPayFinesTaskProvider(Container), new BaseParams());
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
