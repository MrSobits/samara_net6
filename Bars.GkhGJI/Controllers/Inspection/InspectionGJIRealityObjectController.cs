namespace Bars.GkhGji.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class InspectionGjiRealityObjectController : InspectionGjiRealityObjectController<InspectionGjiRealityObject>
    {
    }

    public class InspectionGjiRealityObjectController<T> : B4.Alt.DataController<T>
    where T : InspectionGjiRealityObject
    {
        public ActionResult AddRealityObjects(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IInspectionGjiRealityObjectService>();
            using (this.Container.Using(service))
            {
                var result = service.AddRealityObjects(baseParams);
                return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
            }
        }
    }
}