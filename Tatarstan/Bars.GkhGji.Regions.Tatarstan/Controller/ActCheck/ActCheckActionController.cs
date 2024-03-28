namespace Bars.GkhGji.Regions.Tatarstan.Controller.ActCheck
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.ActCheck;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction;
    using Microsoft.AspNetCore.Mvc;

    public class ActCheckActionController : B4.Alt.DataController<ActCheckAction>
    {
        public ActionResult AddCarriedOutEvents(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IActCheckActionService>();

            using (this.Container.Using(service))
            {
                var result = service.AddCarriedOutEvents(baseParams);
                return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
            }
        }

        public ActionResult AddInspectors(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IActCheckActionService>();

            using (this.Container.Using(service))
            {
                var result = service.AddInspectors(baseParams);
                return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
            }
        }

        public ActionResult GetActionTypes(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IActCheckActionService>();

            using (this.Container.Using(service))
            {
                var result = service.GetActionTypes(baseParams);
                return new JsonListResult(result, result.Count);
            }
        }
    }
}