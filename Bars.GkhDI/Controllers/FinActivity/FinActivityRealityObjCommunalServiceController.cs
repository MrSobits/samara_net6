namespace Bars.GkhDi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using DomainService;
    using Entities;

    public class FinActivityRealityObjCommunalServiceController : B4.Alt.DataController<FinActivityRealityObjCommunalService>
    {
        public ActionResult AddWorkMode(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IFinActivityRealityObjCommunalServService>().AddWorkMode(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult AddDataByRealityObj(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IFinActivityRealityObjCommunalServService>().AddDataByRealityObj(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}
