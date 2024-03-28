namespace Bars.GkhGji.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class ServiceJuridicalGjiController : B4.Alt.DataController<ServiceJuridicalGji>
    {
        public ActionResult AddKindWorkNotification(BaseParams baseParams)
        {
            var result = Container.Resolve<IServiceJuridalGjiService>().AddKindWorkNotification(baseParams);
            return result.Success ? new JsonNetResult(new {success = true}) : JsonNetResult.Failure(result.Message);
        }
    }
}