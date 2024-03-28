namespace Bars.GkhGji.Regions.Chelyabinsk.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using DomainService;
    using Bars.GkhGji.Entities;
    using Entities;
    using Bars.Gkh.Domain;

    public class AppCitOperationsController : BaseController
    {
        public ActionResult CopyAppeal(BaseParams baseParams)
        {
            var service = Container.Resolve<IAppCitOperationsService>();
            try
            {
                var result = service.CopyAppeal(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }


    }
}