namespace Bars.Gkh.Gku.Controllers
{
    using Bars.B4;
    using DomainService;
    using Entities;

    using Microsoft.AspNetCore.Mvc;

    public class GkuTariffGjiController : B4.Alt.DataController<GkuTariffGji>
    {
        public ActionResult GetContragentsList(BaseParams baseParams)
        {
            var result = Resolve<IGkuTariffGjiService>().GetContragents(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}
