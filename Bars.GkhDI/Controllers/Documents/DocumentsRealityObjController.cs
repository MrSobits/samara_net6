namespace Bars.GkhDi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using B4.Modules.FileStorage;
    using DomainService;
    using Entities;

    public class DocumentsRealityObjController : FileStorageDataController<DocumentsRealityObj>
    {
        public ActionResult GetIdByDisnfoId(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IDocumentsRealityObjService>().GetIdByDisnfoId(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult CopyDocs(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IDocumentsRealityObjService>().CopyDocs(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
   }
}