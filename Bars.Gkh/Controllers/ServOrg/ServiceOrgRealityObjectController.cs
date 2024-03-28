namespace Bars.Gkh.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;

    public class ServiceOrgRealityObjectController : B4.Alt.DataController<ServiceOrgRealityObject>
    {
        public ActionResult AddRealityObjects(BaseParams baseParams)
        {
            var result = Container.Resolve<IServiceOrgRealityObjectService>().AddRealityObjects(baseParams);

            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message); 
        }

        public ActionResult GetInfo(BaseParams baseParams)
        {
            var result = Container.Resolve<IServiceOrgRealityObjectService>().GetInfo(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

    }
}
