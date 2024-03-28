namespace Bars.GkhGji.Regions.Nso.Controllers.Disposal
{
	using Bars.B4;
	using Bars.GkhGji.DomainService;
	using Bars.GkhGji.Regions.Nso.Entities.Disposal;
	using Microsoft.AspNetCore.Mvc;

	public class NsoDisposalProvidedDocController : B4.Alt.DataController<NsoDisposalProvidedDoc>
    {
		public IDisposalProvidedDocService DisposalProvidedDocService { get; set; }

        public ActionResult AddProvidedDocuments(BaseParams baseParams)
        {
			var result = DisposalProvidedDocService.AddProvidedDocs(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}