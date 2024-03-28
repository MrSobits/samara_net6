namespace Bars.Gkh.Overhaul.Tat.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Tat.DomainService;
    using Bars.Gkh.Overhaul.Tat.Entities;

    public class RealityObjectStructuralElementInProgrammController : B4.Alt.DataController<RealityObjectStructuralElementInProgramm>
    {
        public ILongProgramService LongProgramService { get; set; }

        public ActionResult MakeStage1(BaseParams baseParams)
        {
            var result = LongProgramService.MakeStage1(baseParams);
            return new JsonNetResult(result);
        }

        public ActionResult MakeStage3(BaseParams baseParams)
        {
            var result = Container.Resolve<IStage2Service>().MakeStage2(baseParams);
            return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult CreateDpkrForPublish(BaseParams baseParams)
        {
            return new JsonNetResult(LongProgramService.CreateDpkrForPublish(baseParams));
        }
    }
}