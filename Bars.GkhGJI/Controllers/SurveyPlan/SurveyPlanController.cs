namespace Bars.GkhGji.Controllers.SurveyPlan
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.DomainService.SurveyPlan;
    using Bars.GkhGji.Entities.SurveyPlan;

    public class SurveyPlanController : B4.Alt.DataController<SurveyPlan>
    {
        public ActionResult AddCandidates(BaseParams baseParams)
        {
            var service = Container.Resolve<ISurveyPlanService>();
            try
            {
                return new JsonNetResult(service.AddCandidates(baseParams));
            }
            finally
            {
                Container.Release(service);
            }
        }

        public ActionResult CreateCandidates(BaseParams baseParams)
        {
            var service = Container.Resolve<ISurveyPlanService>();
            try
            {
                return new JsonNetResult(service.CreateCandidates(baseParams));
            }
            finally
            {
                Container.Release(service);
            }
        }
    }
}