namespace Bars.GkhGji.Regions.Tatarstan.Controller.ActionIsolated
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.ActionIsolated;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;

    public class TaskActionIsolatedItemController : B4.Alt.DataController<TaskActionIsolatedItem>
    {
        public ActionResult AddItems(BaseParams baseParams)
        {
            var preventiveActionTaskItemService = this.Container.Resolve<ITaskActionIsolatedItemService>();

            using (this.Container.Using(preventiveActionTaskItemService))
            {
                return new JsonNetResult(preventiveActionTaskItemService.AddItems(baseParams));
            }
        }
    }
}
