namespace Bars.GkhDi.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhDi.DomainService;
    using Bars.GkhDi.Entities;

    public class DocumentsController : FileStorageDataController<Documents>
    {
        public ActionResult GetIdByDisnfoId(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IDocumentsService>().GetIdByDisnfoId(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult CopyDocs(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IDocumentsService>().CopyDocs(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}