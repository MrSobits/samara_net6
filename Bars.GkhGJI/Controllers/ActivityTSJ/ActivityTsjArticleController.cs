namespace Bars.GkhGji.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class ActivityTsjArticleController : B4.Alt.DataController<ActivityTsjArticle>
    {
        public ActionResult SaveParams(BaseParams baseParams)
        {
            var result = Container.Resolve<IActivityTsjArticleService>().SaveParams(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}