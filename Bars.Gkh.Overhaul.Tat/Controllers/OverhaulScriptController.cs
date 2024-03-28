namespace Bars.Gkh.Overhaul.Tat.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Tat.DomainService;

    /// <summary>
    /// Контроллер для скриптов
    /// </summary>
    public class OverhaulScriptController : BaseController
    {
        public ActionResult CreateStructElements(BaseParams baseParams)
        {
            var result = Container.Resolve<IOverhaulScriptService>().CreateStructElements(baseParams);
            return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult UpdateVolumeStructElements(BaseParams baseParams)
        {
            var result = Container.Resolve<IOverhaulScriptService>().UpdateVolumeStructElements(baseParams);
            return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
        }
    }
}
