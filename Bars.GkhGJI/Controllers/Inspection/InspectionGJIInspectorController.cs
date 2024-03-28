namespace Bars.GkhGji.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class InspectionGjiInspectorController : B4.Alt.DataController<InspectionGjiInspector>
    {
        public ActionResult AddInspectors(BaseParams baseParams)
        {
            var result = Container.Resolve<IInspectionGjiInspectorService>().AddInspectors(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}