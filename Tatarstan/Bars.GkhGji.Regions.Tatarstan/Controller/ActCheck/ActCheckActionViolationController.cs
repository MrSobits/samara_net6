using Bars.B4;
using Bars.B4.IoC;
using Bars.GkhGji.Regions.Tatarstan.DomainService.ActCheck;
using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction;
using Microsoft.AspNetCore.Mvc;

namespace Bars.GkhGji.Regions.Tatarstan.Controller.ActCheck
{
    public class ActCheckActionViolationController : B4.Alt.DataController<ActCheckActionViolation>
    {
        public ActionResult AddViolations(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IActCheckActionViolationService>();

            using (this.Container.Using(service))
            {
                var result = service.AddViolations(baseParams);
                return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
            }
        }
    }
}
