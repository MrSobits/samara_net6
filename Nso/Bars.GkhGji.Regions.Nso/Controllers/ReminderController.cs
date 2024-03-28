// На основе Базового контроллера в GkhGji делаем специально для НСО, посколкьу перекрываются доп. методы
namespace Bars.GkhGji.Regions.Nso.Controllers
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.Regions.Nso.DomainService;

    public class ReminderController : Bars.GkhGji.Controllers.ReminderController
    {
        public ActionResult ListAppealCitsReminder(BaseParams baseParams)
        {
            var service = Container.Resolve<INsoReminderService>();
            try
            {
                var result = (ListDataResult)service.ListAppealCitsReminder(baseParams);
                return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }
    }
}
