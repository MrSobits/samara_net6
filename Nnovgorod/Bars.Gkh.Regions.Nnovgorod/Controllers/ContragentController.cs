namespace Bars.Gkh.Regions.Nnovgorod.Controllers
{
    using Bars.B4;
    using Bars.Gkh.Regions.Nnovgorod.DomainService;

    using Microsoft.AspNetCore.Mvc;

    public class ContragentController : Bars.Gkh.Controllers.ContragentController
    {
        public ActionResult GetActivityInfo(BaseParams baseParams)
        {
            var service = Container.Resolve<IContragentInfoService>();

            try
            {
                var result = service.GetActivityInfo(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : this.JsFailure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }
    }
}