namespace Bars.GkhGji.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class ActSurveyInspectedPartController : B4.Alt.DataController<ActSurveyInspectedPart>
    {
        public ActionResult AddInspectedParts(BaseParams baseParams)
        {
            var result = Container.Resolve<IActSurveyInspectedPartService>().AddInspectedParts(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}