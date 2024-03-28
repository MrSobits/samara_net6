namespace Bars.GkhGji.Regions.Tomsk.Controller
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using Bars.GkhGji.Regions.Tomsk.Entities;

    using DomainService;

    public class AppealCitsAnswerAddresseeController : B4.Alt.DataController<AppealCitsAnswerAddressee>
    {
        public ActionResult AddAddressee(BaseParams baseParams)
        {
            var result = Container.Resolve<IAppealCitsAnswerAddressee>().AddAddressee(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}
