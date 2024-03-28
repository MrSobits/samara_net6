namespace Bars.GkhGji.Regions.Zabaykalye.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.Regions.Zabaykalye.DomainService;
    using Bars.GkhGji.Regions.Zabaykalye.Entities;

    public class DisposalControlMeasuresController : B4.Alt.DataController<DisposalControlMeasures>
    {
        public ActionResult AddDisposalControlMeasures(BaseParams baseParams)
        {
            var result = Container.Resolve<IDisposalControlMeasuresService>().AddDisposalControlMeasures(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}