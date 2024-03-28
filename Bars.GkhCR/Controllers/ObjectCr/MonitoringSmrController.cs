namespace Bars.GkhCr.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhCr.DomainService;
    using Bars.GkhCr.Entities;

    public class MonitoringSmrController : B4.Alt.DataController<MonitoringSmr>
    {
        public ActionResult GetByObjectCrId(BaseParams baseParams)
        {
            var result = (BaseDataResult)Container.Resolve<IMonitoringSmrService>().GetByObjectCrId(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult SaveByObjectCrId(BaseParams baseParams)
        {
            var result = (BaseDataResult)Container.Resolve<IMonitoringSmrService>().SaveByObjectCrId(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}