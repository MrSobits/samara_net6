namespace Bars.GkhDi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using DomainService;
    using Entities;

    public class FinActivityRepairSourceController : B4.Alt.DataController<FinActivityRepairSource>
    {
        public ActionResult AddWorkMode(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IFinActivityRepairSourceService>().AddWorkMode(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult AddDataByRealityObj(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IFinActivityRepairSourceService>().AddDataByRealityObj(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}
