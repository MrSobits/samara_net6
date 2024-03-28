namespace Bars.GkhDi.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhDi.DomainService;
    using Bars.GkhDi.Entities;

    public class FinActivityController : B4.Alt.DataController<FinActivity>
    {
        public ActionResult GetIdByDisnfoId(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IFinActivityService>().GetIdByDisnfoId(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}
