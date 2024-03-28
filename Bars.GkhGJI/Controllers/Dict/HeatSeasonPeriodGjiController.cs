namespace Bars.GkhGji.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.Entities;
    using DomainService;

    public class HeatSeasonPeriodGjiController : B4.Alt.DataController<HeatSeasonPeriodGji>
    {
        public ActionResult GetCurrentPeriod()
        {
            var result = Resolve<IHeatSeasonPeriodGjiService>().GetCurrentPeriod();
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}