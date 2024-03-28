namespace Bars.GkhGji.Regions.Nso.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using Bars.B4;
	using Bars.GkhGji.Regions.Nso.Entities;
	using Bars.GkhGji.Regions.Nso.DomainService;

	public class ActRemovalInspectedPartController : B4.Alt.DataController<ActRemovalInspectedPart>
    {
        public ActionResult AddInspectedParts(BaseParams baseParams)
        {
            var result = Container.Resolve<IActRemovalInspectedPartService>().AddInspectedParts(baseParams);
            return result.Success ? new JsonNetResult(new { succes = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}