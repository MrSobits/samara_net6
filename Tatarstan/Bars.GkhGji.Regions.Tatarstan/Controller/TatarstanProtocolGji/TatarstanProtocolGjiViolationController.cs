namespace Bars.GkhGji.Regions.Tatarstan.Controller.TatarstanProtocolGji
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.TatarstanProtocolGji;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanProtocolGji;

    public class TatarstanProtocolGjiViolationController : B4.Alt.DataController<TatarstanProtocolGjiViolation>
    {
        public ActionResult SaveViolations(BaseParams baseParams)
        {
            var service = this.Container.Resolve<ITatarstanProtocolGjiViolationService>();
            using (this.Container.Using(service))
            {
                var result = service.SaveViolations(baseParams);
                return result.Success ? this.JsSuccess() : this.JsFailure(result.Message);
            }
        }
    }
}
