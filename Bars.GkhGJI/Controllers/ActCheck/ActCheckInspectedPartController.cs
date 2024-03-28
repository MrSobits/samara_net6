namespace Bars.GkhGji.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class ActCheckInspectedPartController : B4.Alt.DataController<ActCheckInspectedPart>
    {
        public ActionResult AddInspectedParts(BaseParams baseParams)
        {
            var result = Container.Resolve<IActCheckInspectedPartService>().AddInspectedParts(baseParams);
            return result.Success ? new JsonNetResult(new { succes = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}