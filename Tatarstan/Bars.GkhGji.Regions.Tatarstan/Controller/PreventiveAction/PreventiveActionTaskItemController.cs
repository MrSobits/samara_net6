namespace Bars.GkhGji.Regions.Tatarstan.Controller.PreventiveAction
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.PreventiveAction;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

    public class PreventiveActionTaskItemController : B4.Alt.DataController<PreventiveActionTaskItem>
    {
        public ActionResult AddItems(BaseParams baseParams)
        {
            var preventiveActionTaskItemService = this.Container.Resolve<IPreventiveActionTaskItemService>();

            using (this.Container.Using(preventiveActionTaskItemService))
            {
                return new JsonNetResult(preventiveActionTaskItemService.AddItems(baseParams));
            }
        }
    }
}