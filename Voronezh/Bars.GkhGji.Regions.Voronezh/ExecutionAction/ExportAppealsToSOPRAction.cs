using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.DomainService;
using Bars.Gkh.Entities;
using Bars.Gkh.ExecutionAction;
using Bars.GkhGji.Regions.Voronezh.DomainService;
using Bars.GkhGji.Regions.Voronezh.Tasks;
using System;
using System.Linq;

namespace Bars.GkhGji.Regions.Voronezh.ExecutionAction
{
    /// <summary>
    /// Сводные действия для ГЖИ Воронеж
    /// </summary>
    public class ExportAppealsToSOPRAction : BaseExecutionAction//, IMandatoryExecutionAction
    {
        public override string Description => "Сводные действия для ГЖИ Воронеж";

        public override string Name => "Универсальные действия для ГЖИ Воронеж";

        public override Func<IDataResult> Action => ExportAppealsToSOPR;

        //public bool IsNeedAction() => true;


        private IDataResult ExportAppealsToSOPR()
        {
            var service = Container.Resolve<ISSTUExportTaskAppealService>();
            var personService = Container.Resolve<IPersonService>();
            try
            {
                var result = service.ActualizeSopr(new BaseParams());
                //taskManager.CreateTasks(new FixRemindersTaskProvider(Container), new BaseParams());
                //return new BaseDataResult(true, "Задача успешно поставлена");
                personService.ChangeOfCertificateStatus();
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
