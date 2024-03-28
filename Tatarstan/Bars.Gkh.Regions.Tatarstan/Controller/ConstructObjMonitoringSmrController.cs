namespace Bars.Gkh.Regions.Tatarstan.Controller
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Regions.Tatarstan.DomainService;
    using Bars.Gkh.Regions.Tatarstan.Entities;

    public class ConstructObjMonitoringSmrController : B4.Alt.DataController<ConstructObjMonitoringSmr>
    {
        public ActionResult GetByConstructObjectId(BaseParams baseParams)
        {
            var result = (BaseDataResult)this.Container.Resolve<IConstructObjMonitoringSmrService>().GetByConstructObjectId(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult SaveByConstructObjectId(BaseParams baseParams)
        {
            var result = (BaseDataResult)this.Container.Resolve<IConstructObjMonitoringSmrService>().SaveByConstructObjectId(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}