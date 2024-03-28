// На основе Базового контроллера в GkhGji делаем специально для НСО, посколкьу перекрываются доп. методы
namespace Bars.GkhGji.Regions.BaseChelyabinsk.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Regions.BaseChelyabinsk.DomainService;

    public class ReminderController : Bars.GkhGji.Controllers.ReminderController
    {
        public ActionResult ListAppealCitsReminder(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IExtReminderService>();
            try
            {
                return service.ListAppealCitsReminder(baseParams).ToJsonResult();
            }
            finally
            {
                this.Container.Release(service);
            }
        }
    }
}
