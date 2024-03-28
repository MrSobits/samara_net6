namespace Bars.GkhDi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.GkhDi.DomainService;
    using Bars.GkhDi.Entities;

    public class RealityObjGroupController : B4.Alt.DataController<RealityObjGroup>
    {
        public ActionResult AddRealityObj(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IRealityObjGroupService>().AddRealityObj(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}
