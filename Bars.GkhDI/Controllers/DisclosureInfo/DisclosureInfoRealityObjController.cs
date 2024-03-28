namespace Bars.GkhDi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using DomainService;
    using Entities;
    using Gkh.Domain;

    public class DisclosureInfoRealityObjController : B4.Alt.DataController<DisclosureInfoRealityObj>
    {
        public ActionResult GetDisclosureInfo(BaseParams baseParams)
        {
            var result = (BaseDataResult) this.Resolve<IDisclosureInfoRealityObjService>().GetDisclosureInfo(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult GetCopyInfo(BaseParams baseParams)
        {
            var result = (BaseDataResult) this.Resolve<IDisclosureInfoRealityObjService>().GetCopyInfo(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult SaveDisclosureInfo(BaseParams baseParams)
        {
            var result = (BaseDataResult) this.Resolve<IDisclosureInfoRealityObjService>().SaveDisclosureInfo(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult GetRealtyObjectPassport(BaseParams baseParams)
        {
            var result = this.Resolve<IDisclosureInfoRealityObjService>().GetRealtyObjectPassport(baseParams);
            return result.ToJsonResult();
        }

        public ActionResult GetRealtyObjectDevices(BaseParams baseParams)
        {
            var result = this.Resolve<IDisclosureInfoRealityObjService>().GetRealtyObjectDevices(baseParams);
            return result.ToJsonResult();
        }

        public ActionResult GetRealtyObjectLifts(BaseParams baseParams)
        {
            var result = this.Resolve<IDisclosureInfoRealityObjService>().GetRealtyObjectLifts(baseParams);
            return result.ToJsonResult();
        }

        public ActionResult GetRealtyObjectStructElements(BaseParams baseParams)
        {
            var result = this.Resolve<IDisclosureInfoRealityObjService>().GetRealtyObjectStructElements(baseParams);
            return result.ToJsonResult();
        }

        public ActionResult GetRealtyObjectEngineerSystems(BaseParams baseParams)
        {
            var result = this.Resolve<IDisclosureInfoRealityObjService>().GetRealtyObjectEngineerSystems(baseParams);
            return result.ToJsonResult();
        }

        public ActionResult GetRealtyObjectHouseManaging(BaseParams baseParams)
        {
            var result = (BaseDataResult) this.Resolve<IDisclosureInfoRealityObjService>().GetRealtyObjectHouseManaging(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}
