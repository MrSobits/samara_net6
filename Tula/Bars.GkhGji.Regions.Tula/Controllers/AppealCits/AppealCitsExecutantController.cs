namespace Bars.GkhGji.Regions.Tula.Controllers
{
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.GkhGji.Regions.Tula.DomainService;
    using Bars.GkhGji.Regions.Tula.Entities;

    public class AppealCitsExecutantController : B4.Alt.DataController<AppealCitsExecutant>
    {
        public ActionResult AddExecutants(BaseParams baseParams)
        {

            var service = Container.Resolve<IAppealCitsExecutantService>();
            ActionResult actionResult;
            using (Container.Using(service))
            {
                var result = service.AddExecutants(baseParams);
                actionResult = result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
            }

            return actionResult;
        }

        public ActionResult RedirectExecutant(BaseParams baseParams)
        {

            var service = Container.Resolve<IAppealCitsExecutantService>();
            ActionResult actionResult;
            using (Container.Using(service))
            {
                var result = Container.Resolve<IAppealCitsExecutantService>().RedirectExecutant(baseParams);
                actionResult = result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
            }

            return actionResult;
        }
    }
}