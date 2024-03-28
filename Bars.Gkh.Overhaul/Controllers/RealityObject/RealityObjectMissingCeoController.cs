namespace Bars.Gkh.Overhaul.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using DomainService;
    using Entities;

    public class RealityObjectMissingCeoController : B4.Alt.DataController<RealityObjectMissingCeo>
    {
        public virtual ActionResult AddMissingCeo(BaseParams baseParams)
        {
            var result = Container.Resolve<IRealityObjectMissingCeoService>().AddMissingCeo(baseParams);
            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        }
    }
}