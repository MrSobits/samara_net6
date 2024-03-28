namespace Bars.GkhDi.Controllers
{
    using B4;
    using B4.Modules.FileStorage;
    using DomainService;
    using Entities;

    using Microsoft.AspNetCore.Mvc;

    public class AdminRespController : FileStorageDataController<AdminResp>
    {
        public ActionResult AddAdminRespByResolution(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IAdminRespService>().AddAdminRespByResolution(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}