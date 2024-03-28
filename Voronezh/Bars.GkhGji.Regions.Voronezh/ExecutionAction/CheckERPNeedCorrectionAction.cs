using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.Entities;
using Bars.Gkh.ExecutionAction;
using Bars.GkhGji.Regions.Voronezh.Tasks;
using System;
using System.Linq;

namespace Bars.GkhGji.Regions.Voronezh.ExecutionAction
{
    /// <summary>
    /// Периодическая проверка реестра ЕРП на необходимость коррекции проверок
    /// </summary>
    public class CheckERPNeedCorrectionAction : BaseExecutionAction//, IMandatoryExecutionAction
    {
        public override string Description => "Проставляет признак необходимости коррекции отправленных ранее проверок";

        public override string Name => "Актуализировать реестр ЕРП для проверок, которым необходима коррекция";

        public override Func<IDataResult> Action => CheckERPNeedCorrection;

        //public bool IsNeedAction() => true;


        private IDataResult CheckERPNeedCorrection()
        {
            var taskManager = Container.Resolve<ITaskManager>();

            try
            {

                var baseParams = new BaseParams();              

                taskManager.CreateTasks(new CheckERPNeedCorrectionTaskProvider(Container), baseParams);
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
