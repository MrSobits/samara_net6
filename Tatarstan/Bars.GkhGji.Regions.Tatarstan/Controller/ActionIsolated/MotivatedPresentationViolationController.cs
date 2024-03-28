namespace Bars.GkhGji.Regions.Tatarstan.Controller.ActionIsolated
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;
    using Bars.GkhGji.Regions.Tatarstan.ViewModel.ActionIsolated;

    public class MotivatedPresentationViolationController : B4.Alt.DataController<MotivatedPresentationViolation>
    {
        public ActionResult RealityObjectsListForMotivatedPresentationActionToWarningDocRule(BaseParams baseParams)
        {
            return new JsonListResult((IEnumerable)(this.ViewModel as MotivatedPresentationViolationViewModel)
                .RealityObjectsListForMotivatedPresentationActionToWarningDocRule(this.DomainService, baseParams).Data);
        }
    }
}