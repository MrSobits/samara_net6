namespace Bars.GkhGji.Regions.Nso.Controllers
{
	using Bars.GkhGji.Regions.Nso.DomainService;
	using Microsoft.AspNetCore.Mvc;
	using Bars.B4;
	using Bars.GkhGji.Regions.Nso.Entities;

	public class Protocol197ArticleLawController : B4.Alt.DataController<Protocol197ArticleLaw>
    {
        public ActionResult AddArticles(BaseParams baseParams)
        {
            var result = Container.Resolve<IProtocol197ArticleLawService>().AddArticles(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}