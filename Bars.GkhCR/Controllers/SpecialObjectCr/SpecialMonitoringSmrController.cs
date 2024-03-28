namespace Bars.GkhCr.Controllers
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.GkhCr.DomainService;
    using Bars.GkhCr.Entities;

    using Microsoft.AspNetCore.Mvc;

    public class SpecialMonitoringSmrController : B4.Alt.DataController<SpecialMonitoringSmr>
    {
        public ActionResult GetByObjectCrId(BaseParams baseParams)
        {
            var service = this.Container.Resolve<ISpecialMonitoringSmrService>();
            using (this.Container.Using(service))
            {
                var result = (BaseDataResult)service.GetByObjectCrId(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
        }

        public ActionResult SaveByObjectCrId(BaseParams baseParams)
        {
            var service = this.Container.Resolve<ISpecialMonitoringSmrService>();
            using (this.Container.Using(service))
            {
                var result = (BaseDataResult) service.SaveByObjectCrId(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
        }
    }
}