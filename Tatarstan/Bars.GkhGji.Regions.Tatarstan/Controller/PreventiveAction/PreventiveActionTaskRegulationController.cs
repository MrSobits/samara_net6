namespace Bars.GkhGji.Regions.Tatarstan.Controller.PreventiveAction
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.PreventiveAction;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

    public class PreventiveActionTaskRegulationController : B4.Alt.DataController<PreventiveActionTaskRegulation>
    {
        public ActionResult AddNormativeDocs(BaseParams baseParams)
        {
            var preventiveActionTaskRegulationService = this.Container.Resolve<IPreventiveActionTaskRegulationService>();

            using (this.Container.Using(preventiveActionTaskRegulationService))
            {
                return new JsonNetResult(preventiveActionTaskRegulationService.AddNormativeDocs(baseParams));
            }
        }
    }
}