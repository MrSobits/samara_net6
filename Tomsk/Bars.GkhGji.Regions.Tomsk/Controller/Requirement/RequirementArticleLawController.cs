namespace Bars.GkhGji.Regions.Tomsk.Controller
{
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.GkhGji.Regions.Tomsk.DomainService;
    using Bars.GkhGji.Regions.Tomsk.Entities;

    public class RequirementArticleLawController : B4.Alt.DataController<RequirementArticleLaw>
    {
        public ActionResult AddArticles(BaseParams baseParams)
        {
            var result = Container.Resolve<IRequirementArticleLawService>().AddArticles(baseParams);
            return result.Success ? JsSuccess() : JsFailure(result.Message);
        }
    }
}
