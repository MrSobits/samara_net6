namespace Bars.GkhGji.Regions.Tatarstan.Controller.ActionIsolated
{
    using Bars.B4;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;
    using Microsoft.AspNetCore.Mvc;

    using Bars.GkhGji.Regions.Tatarstan.DomainService.ActionIsolated;
    using Bars.B4.IoC;

    public class TaskActionIsolatedRealityObjectController : B4.Alt.DataController<TaskActionIsolatedRealityObject>
    {
        public ActionResult AddRealityObjects(BaseParams baseParams)
        {
            var service = this.Container.Resolve<ITaskActionIsolatedRealityObjectService>();
            using (this.Container.Using(service))
            {
                var result = service.AddRealityObjects(baseParams);

                return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
            }
        }
    }
}