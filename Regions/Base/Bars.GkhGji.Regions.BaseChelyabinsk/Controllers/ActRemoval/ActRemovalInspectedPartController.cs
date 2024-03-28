namespace Bars.GkhGji.Regions.BaseChelyabinsk.Controllers.ActRemoval
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.Regions.BaseChelyabinsk.DomainService;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActRemoval;

    public class ActRemovalInspectedPartController : B4.Alt.DataController<ActRemovalInspectedPart>
    {
        public ActionResult AddInspectedParts(BaseParams baseParams)
        {
            var result = this.Container.Resolve<IActRemovalInspectedPartService>().AddInspectedParts(baseParams);
            return result.Success ? new JsonNetResult(new { succes = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}