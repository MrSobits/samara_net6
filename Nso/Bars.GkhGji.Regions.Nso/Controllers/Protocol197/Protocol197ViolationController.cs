namespace Bars.GkhGji.Regions.Nso.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using B4;
	using DomainService;
	using Entities;

	public class Protocol197ViolationController : B4.Alt.DataController<Protocol197Violation>
    {
		public ActionResult Save(BaseParams baseParams)
		{
			var result = Container.Resolve<IProtocol197ViolationService>().Save(baseParams);
			return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
		}
    }
}