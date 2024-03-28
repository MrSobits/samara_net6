namespace Bars.GkhDi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using B4.Modules.FileStorage;
    using DomainService;
    using Entities;

    public class DisclosureInfoController : FileStorageDataController<DisclosureInfo>
    {
        public ActionResult GetDisclosureInfo(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IDisclosureInfoService>().GetDisclosureInfo(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult GetDisclosureOfManOrg(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IDisclosureInfoService>().GetDisclosureOfManOrg(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult GetOperatorManOrg(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IDisclosureInfoService>().GetOperatorManOrg(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult GetDateStartByPeriod(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IDisclosureInfoService>().GetDateStartByPeriod(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult GetTypeManagingByDisinfo(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IDisclosureInfoService>().GetTypeManagingByDisinfo(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}
