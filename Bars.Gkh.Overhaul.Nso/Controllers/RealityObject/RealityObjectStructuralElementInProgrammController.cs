namespace Bars.Gkh.Overhaul.Nso.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Nso.DomainService;
    using Bars.Gkh.Overhaul.Nso.Entities;

    public class RealityObjectStructuralElementInProgrammController : B4.Alt.DataController<RealityObjectStructuralElementInProgramm>
    {
        public ILongProgramService LongProgramService { get; set; }

        public ActionResult MakeLongProgram(BaseParams baseParams)
        {
            var result = LongProgramService.MakeLongProgram(baseParams);
            return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult CreateDpkrForPublish(BaseParams baseParams)
        {
            var result = LongProgramService.CreateDpkrForPublish(baseParams);
            return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
        }
    }
}