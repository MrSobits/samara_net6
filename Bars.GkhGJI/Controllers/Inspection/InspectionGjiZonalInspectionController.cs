namespace Bars.GkhGji.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class InspectionGjiZonalInspectionController : B4.Alt.DataController<InspectionGjiZonalInspection>
    {
        public ActionResult AddZonalInspections(BaseParams baseParams)
        {
            var result = Container.Resolve<IInspectionGjiZonalInspectionService>().AddZonalInspections(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}