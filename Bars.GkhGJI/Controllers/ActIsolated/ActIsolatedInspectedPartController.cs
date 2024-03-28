namespace Bars.GkhGji.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class ActIsolatedInspectedPartController : B4.Alt.DataController<ActIsolatedInspectedPart>
    {
        public ActionResult AddInspectedParts(BaseParams baseParams)
        {
            var result = this.Container.Resolve<IActIsolatedInspectedPartService>().AddInspectedParts(baseParams);
            return result.Success ? new JsonNetResult(new { succes = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}