namespace Bars.GkhGji.Regions.BaseChelyabinsk.Controllers.Protocol197
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.Regions.BaseChelyabinsk.DomainService;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;

    public class Protocol197ArticleLawController : B4.Alt.DataController<Protocol197ArticleLaw>
    {
        public ActionResult AddArticles(BaseParams baseParams)
        {
            var result = this.Container.Resolve<IProtocol197ArticleLawService>().AddArticles(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}