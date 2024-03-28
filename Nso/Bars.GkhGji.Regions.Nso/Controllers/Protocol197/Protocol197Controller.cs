namespace Bars.GkhGji.Regions.Nso.Controllers
{
	using System.Collections;
	using Bars.GkhGji.Regions.Nso.DomainService;
	using B4;
	using B4.Modules.DataExport.Domain;
	using Entities;
	using Microsoft.AspNetCore.Mvc;
	using Bars.Gkh.DomainService;

	public class Protocol197Controller : B4.Alt.DataController<Protocol197>
	{
		public IBlobPropertyService<Protocol197, Protocol197LongText> LongTextService { get; set; }

		public ActionResult Export(BaseParams baseParams)
		{
			var export = Container.Resolve<IDataExportService>("Protocol197DataExport");
			return export != null ? export.ExportData(baseParams) : null;
		}

		public ActionResult ListView(BaseParams baseParams)
		{
			var result = (ListDataResult)Container.Resolve<IProtocol197Service>().ListView(baseParams);
			return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);
		}

		public ActionResult GetInfo(long? documentId)
		{
			var result = Container.Resolve<IProtocol197Service>().GetInfo(documentId);
			return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
		}

		public ActionResult AddRequirements(BaseParams baseParams)
		{
			var result = Container.Resolve<IProtocol197Service>().AddRequirements(baseParams);
			return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
		}

		public ActionResult AddDirections(BaseParams baseParams)
		{
			var result = Container.Resolve<IProtocol197Service>().AddDirections(baseParams);
			return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
		}

		public virtual ActionResult GetDescription(BaseParams baseParams)
		{
			var result = LongTextService.Get(baseParams);
			return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
		}

		public virtual ActionResult SaveDescription(BaseParams baseParams)
		{
			var result = LongTextService.Save(baseParams);
			return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
		}
	}
}