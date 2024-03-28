namespace Bars.Gkh.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;

    public class RealityObjectCouncillorsController : B4.Alt.DataController<RealityObjectCouncillors>
    {
        public ActionResult IsShowConcillors(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IRealityObjectCouncillorsService>().IsShowConcillors(baseParams);
            if (result.Success)
            {
                return new JsonNetResult(result);
            }

            return JsonNetResult.Failure(result.Message);
        }
    }
}
