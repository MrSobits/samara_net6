namespace Bars.GkhGji.Regions.Tatarstan.Controller.TatarstanProtocolGji
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.TatarstanProtocolGji;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanProtocolGji;

    public class TatarstanProtocolGjiArticleLawController : B4.Alt.DataController<TatarstanProtocolGjiArticleLaw>
    {
        public ActionResult SaveArticles(BaseParams baseParams)
        {
            var service = this.Container.Resolve<ITatarstanProtocolGjiArticleLawService>();
            using (this.Container.Using(service))
            {
                var result = service.SaveArticles(baseParams);
                return result.Success ? this.JsSuccess() : this.JsFailure(result.Message);
            }
        }
    }
}
