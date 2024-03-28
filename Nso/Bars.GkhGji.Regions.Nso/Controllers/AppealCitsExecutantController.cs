namespace Bars.GkhGji.Regions.Nso.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.Regions.Nso.DomainService;
    using Bars.GkhGji.Regions.Nso.Entities;

    public class AppealCitsExecutantController : B4.Alt.DataController<AppealCitsExecutant>
    {
        public ActionResult AddExecutants(BaseParams baseParams)
        {
            var result = Container.Resolve<IAppealCitsExecutantService>().AddExecutants(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult RedirectExecutant(BaseParams baseParams)
        {
            var result = Container.Resolve<IAppealCitsExecutantService>().RedirectExecutant(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}