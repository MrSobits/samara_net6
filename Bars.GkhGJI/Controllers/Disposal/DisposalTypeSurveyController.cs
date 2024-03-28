namespace Bars.GkhGji.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class DisposalTypeSurveyController : B4.Alt.DataController<DisposalTypeSurvey>
    {
        public ActionResult AddTypeSurveys(BaseParams baseParams)
        {
            var result = Container.Resolve<IDisposalTypeSurveyService>().AddTypeSurveys(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}