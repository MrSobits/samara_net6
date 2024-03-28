namespace Bars.GkhGji.Regions.Zabaykalye.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.Regions.Zabaykalye.DomainService;
    using Bars.GkhGji.Regions.Zabaykalye.Entities;

    public class DisposalSurveySubjectController : B4.Alt.DataController<DisposalSurveySubject>
    {
        public IDisposalSurveySubjectService Service { get; set; }

        public ActionResult AddDisposalSurveySubject(BaseParams baseParams)
        {
            var result = Service.AddDisposalSurveySubject(baseParams);

            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}