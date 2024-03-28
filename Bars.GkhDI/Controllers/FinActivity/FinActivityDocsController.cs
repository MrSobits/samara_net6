namespace Bars.GkhDi.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhDi.DomainService;
    using Bars.GkhDi.Entities;

    public class FinActivityDocsController : FileStorageDataController<FinActivityDocs>
    {
        public ActionResult GetIdByDisnfoId(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IFinActivityDocsService>().GetIdByDisnfoId(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}
