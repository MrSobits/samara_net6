namespace Bars.GkhGji.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class DisposalProvidedDocController : B4.Alt.DataController<DisposalProvidedDoc>
    {
        public ActionResult AddProvidedDocuments(BaseParams baseParams)
        {
            var result = Container.Resolve<IDisposalProvidedDocService>().AddProvidedDocs(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult AutoAddProvidedDocuments(BaseParams baseParams)
        {
            var result = Container.Resolve<IDisposalService>().AutoAddProvidedDocuments(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}