namespace Bars.Gkh.Overhaul.Hmao.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using DomainService;
    using Entities;

    public class PropertyOwnerProtocolInspectorController : B4.Alt.DataController<PropertyOwnerProtocolInspector>
    {
        public ActionResult AddInspectors(BaseParams baseParams)
        {
            var result = Container.Resolve<IPropertyOwnerProtocolInspectorService>().AddInspectors(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult GetInfo(BaseParams baseParams)
        {
            return Container.Resolve<IPropertyOwnerProtocolInspectorService>().GetInfo(baseParams).ToJsonResult();
        }

        public ActionResult AddDicisions(BaseParams baseParams)
        {
            var result = Container.Resolve<IPropertyOwnerProtocolInspectorService>().AddDicisions(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}