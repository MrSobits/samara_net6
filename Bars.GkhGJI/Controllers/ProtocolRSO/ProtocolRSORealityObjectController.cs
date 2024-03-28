namespace Bars.GkhGji.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class ProtocolRSORealityObjectController : B4.Alt.DataController<ProtocolRSORealityObject>
    {
        public ActionResult AddRealityObjects(BaseParams baseParams)
        {
            var service = Container.Resolve<IProtocolRSORealityObjectService>();
            try
            {
                var result = service.AddRealityObjects(baseParams);
                return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
            }
            finally 
            {
                Container.Release(service);
            }
        }
    }
}