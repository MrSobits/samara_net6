namespace Bars.Gkh.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using DomainService;
    using Entities.Hcs;

    public class HouseInfoOverviewController : B4.Alt.DataController<HouseInfoOverview>
    {
        public ActionResult GetHouseInfoOverviewByRealityObjectId(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IHouseInfoOverviewService>().GetHouseInfoOverviewByRealityObjectId(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}