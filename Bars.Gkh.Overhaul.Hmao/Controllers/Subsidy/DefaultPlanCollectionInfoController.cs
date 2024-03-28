namespace Bars.Gkh.Overhaul.Hmao.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using B4;
    using Entities;
    using DomainService;

    public class DefaultPlanCollectionInfoController : B4.Alt.DataController<DefaultPlanCollectionInfo>
    {
        public IDefaultPlanCollectionInfoService Service { get; set; }

        public ActionResult UpdatePeriod(BaseParams baseParams)
        {
            var result = Service.UpdatePeriod(baseParams);
            return result.Success ? JsSuccess(result) : JsFailure(result.Message);
        }

        public ActionResult CopyCollectionInfo(BaseParams baseParams)
        {
            var result = Service.CopyCollectionInfo(baseParams);
            return result.Success ? JsSuccess(result) : JsFailure(result.Message);
        }
    }
}