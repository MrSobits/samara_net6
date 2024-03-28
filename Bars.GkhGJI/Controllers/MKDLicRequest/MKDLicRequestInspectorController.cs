namespace Bars.GkhGji.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using DomainService;
    using Entities;

    public class MKDLicRequestInspectorController : B4.Alt.DataController<MKDLicRequestInspector>
    {
        public ActionResult AddInspectors(BaseParams baseParams)
        {
            var result = Container.Resolve<IMKDLicRequestInspectorService>().AddInspectors(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult GetInfo(BaseParams baseParams)
        {
            return Container.Resolve<IMKDLicRequestInspectorService>().GetInfo(baseParams).ToJsonResult();
        }
    }
}