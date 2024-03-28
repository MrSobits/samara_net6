namespace Bars.GkhDi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using DomainService;
    using Entities;

    public class FinActivityCommunalServiceController : B4.Alt.DataController<FinActivityCommunalService>
    {
        public ActionResult AddWorkMode(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IFinActivityCommunalServService>().AddWorkMode(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}
