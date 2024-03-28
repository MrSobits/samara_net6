using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.Entities;
using Bars.Gkh.ExecutionAction;
using Bars.GkhGji.Regions.Chelyabinsk.DomainService;
using Bars.GkhGji.Regions.Chelyabinsk.Tasks;
using System;
using System.Linq;

namespace Bars.GkhGji.Regions.Chelyabinsk.ExecutionAction
{
    /// <summary>
    /// Периодическая проверка реестра обращений для экспорта данных в СОПР
    /// </summary>
    public class ExportAppealsToSOPRAction : BaseExecutionAction//, IMandatoryExecutionAction
    {
        public override string Description => "Отправляет данные по обращению в реестр СОПР";

        public override string Name => "Автоматическая отправка обращений в СОПР";

        public override Func<IDataResult> Action => ExportAppealsToSOPR;

        //public bool IsNeedAction() => true;


        private IDataResult ExportAppealsToSOPR()
        {
            var service = Container.Resolve<ISSTUExportTaskAppealService>();
            try
            {
                var result = service.ActualizeSopr(new BaseParams());
                //taskManager.CreateTasks(new FixRemindersTaskProvider(Container), new BaseParams());
                //return new BaseDataResult(true, "Задача успешно поставлена");
                return result;
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, e.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }
    }
}
