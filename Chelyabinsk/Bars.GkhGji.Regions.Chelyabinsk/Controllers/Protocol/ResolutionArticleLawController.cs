namespace Bars.GkhGji.Regions.Chelyabinsk.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using DomainService;
    using Bars.GkhGji.Entities;
    using Entities;


    public class ResolutionArticleLawController : B4.Alt.DataController<ResolutionArtLaw>
    {
        public ActionResult AddArticles(BaseParams baseParams)
        {
            var result = Container.Resolve<IResolutionArticleLawService>().AddArticles(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}