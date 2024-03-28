
namespace Bars.GkhRf.Controllers
{
    using B4;
    using Microsoft.AspNetCore.Mvc;
    using Entities;
    using DomainService;

    public class LimitCheckFinSourceController : B4.Alt.DataController<LimitCheckFinSource>
    {
        public ActionResult AddFinSources(BaseParams baseParams)
        {
            var result = Resolve<ILimitCheckFinSourceService>().AddFinSources(baseParams);
            return result.Success ? new JsonNetResult(new {success = true}) : JsonNetResult.Failure(result.Message);
        }
    }
}