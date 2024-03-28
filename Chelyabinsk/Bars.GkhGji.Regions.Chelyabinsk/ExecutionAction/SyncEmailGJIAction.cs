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
    public class SyncEmailGJIAction : BaseExecutionAction//, IMandatoryExecutionAction
    {
        public override string Description => "Получаем почту для ГЖИ";

        public override string Name => "Получение входящей почты";

        public override Func<IDataResult> Action => SyncEmailGJI;

        //public bool IsNeedAction() => true;


        private IDataResult SyncEmailGJI()
        {
            var service = Container.Resolve<IAppCitOperationsService>();
            try
            {
                var result = service.SyncEmailGJI(new BaseParams());
           
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
