namespace Bars.GkhGji.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class AppealCitsRealObjectController : B4.Alt.DataController<AppealCitsRealityObject>
    {
        public ActionResult AddRealityObjects(BaseParams baseParams)
        {
            var result = Container.Resolve<IAppealCitsRealObjService>().AddRealityObjects(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult AddStatementRealityObjects(BaseParams baseParams)
        {
            var result = Container.Resolve<IAppealCitsRealObjService>().AddStatementRealityObjects(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult GetRealityObjects(BaseParams baseParams)
        {
            var result = Container.Resolve<IAppealCitsRealObjService>().GetRealityObjects(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult GetJurOrgs(BaseParams baseParams)
        {
            var result = Container.Resolve<IAppealCitsRealObjService>().GetJurOrgs(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}