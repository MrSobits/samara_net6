namespace Bars.GkhGji.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class ViolationActionsRemovGjiController : B4.Alt.DataController<ViolationActionsRemovGji>
    {
        public ActionResult AddViolationActionsRemov(BaseParams baseParams)
        {
            var result = Container.Resolve<IViolationActionsRemovGjiService>().AddViolationActionsRemov(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}
