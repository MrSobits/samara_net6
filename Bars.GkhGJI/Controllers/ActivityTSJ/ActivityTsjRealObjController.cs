namespace Bars.GkhGji.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class ActivityTsjRealObjController : B4.Alt.DataController<ActivityTsjRealObj>
    {
        public ActionResult AddRealityObjects(BaseParams baseParams)
        {
            var result = Container.Resolve<IActivityTsjRealObjService>().AddRealityObjects(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult GetInfo(long? activityTsjId)
        {
            var result = Container.Resolve<IActivityTsjRealObjService>().GetInfo(activityTsjId);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}