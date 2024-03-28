// На основе Базового контроллера в GkhGji делаем специально для Tomsk, посколкьу перекрываются доп. методы
namespace Bars.GkhGji.Regions.Tomsk.Controllers
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.Regions.Tomsk.DomainService;

    public class ReminderController : Bars.GkhGji.Controllers.ReminderController
    {
        public ActionResult ListAppealCitsReminder(BaseParams baseParams)
        {
            var service = Container.Resolve<ITomskReminderService>();
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
