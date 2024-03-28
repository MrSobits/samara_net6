namespace Bars.GkhDi.Regions.Tatarstan.Controllers
{
    using Bars.B4;
    using Bars.GkhDi.Regions.Tatarstan.DomainService;
    using Bars.GkhDi.Regions.Tatarstan.Entities;

    using Microsoft.AspNetCore.Mvc;

    public class PlanReduceMeasureNameController : B4.Alt.DataController<PlanReduceMeasureName>
    {
        public ActionResult AddPlanReduceMeasureName(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IPlanReduceMeasureNameService>().AddPlanReduceMeasureName(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}
