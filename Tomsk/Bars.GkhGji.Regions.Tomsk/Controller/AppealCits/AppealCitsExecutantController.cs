namespace Bars.GkhGji.Regions.Tomsk.Controller.AppealCits
{
	using Microsoft.AspNetCore.Mvc;
	using Bars.B4;
	using Bars.B4.IoC;
	using Bars.B4.Modules.FileStorage;
	using Bars.GkhGji.Regions.Tomsk.Entities.AppealCits;
	using Bars.GkhGji.Regions.Tomsk.DomainService.AppealCits;

	public class AppealCitsExecutantController : FileStorageDataController<AppealCitsExecutant>
    {
        public ActionResult AddExecutants(BaseParams baseParams)
        {
            var service = Container.Resolve<IAppealCitsExecutantService>();
            ActionResult actionResult;
            using (Container.Using(service))
            {
                var result = service.AddExecutants(baseParams);
                actionResult = result.Success
                                   ? new JsonNetResult(new {success = true})
                                   : JsonNetResult.Failure(result.Message);
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
                actionResult = result.Success
                                   ? new JsonNetResult(new {success = true})
                                   : JsonNetResult.Failure(result.Message);
            }

            return actionResult;
        }
    }
}