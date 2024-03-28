namespace Bars.Gkh.Overhaul.Hmao.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Hmao.DomainService;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    public class RealityObjectStructuralElementInProgrammController : B4.Alt.DataController<RealityObjectStructuralElementInProgramm>
    {
        public ILongProgramService LongProgramService { get; set; }

        public ActionResult CreateDpkrForPublish(BaseParams baseParams)
        {
            var result = LongProgramService.CreateDpkrForPublish(baseParams);
            return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
        }
    }
}