namespace Bars.GkhGji.Regions.BaseChelyabinsk.Controllers.Disposal
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Disposal;

    public class ChelyabinskDisposalProvidedDocController : B4.Alt.DataController<ChelyabinskDisposalProvidedDoc>
    {
		public IDisposalProvidedDocService DisposalProvidedDocService { get; set; }

        public ActionResult AddProvidedDocuments(BaseParams baseParams)
        {
			var result = this.DisposalProvidedDocService.AddProvidedDocs(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}