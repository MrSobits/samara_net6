namespace Bars.GkhGji.Regions.BaseChelyabinsk.Controllers.Protocol197
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.Regions.BaseChelyabinsk.DomainService;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;

    public class Protocol197ViolationController : B4.Alt.DataController<Protocol197Violation>
    {
		public ActionResult Save(BaseParams baseParams)
		{
			var result = this.Container.Resolve<IProtocol197ViolationService>().Save(baseParams);
			return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
		}
    }
}