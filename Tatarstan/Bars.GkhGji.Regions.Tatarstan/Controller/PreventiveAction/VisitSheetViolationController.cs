namespace Bars.GkhGji.Regions.Tatarstan.Controller.PreventiveAction
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.PreventiveAction;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

    /// <summary>
    /// Контроллер для <see cref="VisitSheetViolation"/>
    /// </summary>
    public class VisitSheetViolationController : B4.Alt.DataController<VisitSheetViolation>
    {
        /// <summary>
        /// Добавить нарушения
        /// </summary>
        public ActionResult AddViolations(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IVisitSheetViolationService>();
            using (this.Container.Using(service))
            {
                return service.AddViolations(baseParams).ToJsonResult();
            }
        }
    }
}