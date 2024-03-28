namespace Bars.GkhGji.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities.Dict;

    public class ViolationNormativeDocItemGjiController : B4.Alt.DataController<ViolationNormativeDocItemGji>
    {
        public ActionResult ListTree(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IViolationNormativeDocItemService>().ListTree(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult SaveNormativeDocItems(BaseParams baseParams)
        {
            var result = Container.Resolve<IViolationNormativeDocItemService>().SaveNormativeDocItems(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}