namespace Bars.GkhGji.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class ResolProsRealityObjectController : B4.Alt.DataController<ResolProsRealityObject>
    {
        public ActionResult AddRealityObjects(BaseParams baseParams)
        {
            var result = Container.Resolve<IResolProsRealityObjectService>().AddRealityObjects(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}