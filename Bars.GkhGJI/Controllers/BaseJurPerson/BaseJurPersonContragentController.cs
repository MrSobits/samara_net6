namespace Bars.GkhGji.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class BaseJurPersonContragentController : B4.Alt.DataController<BaseJurPersonContragent>
    {
        public ActionResult AddContragents(BaseParams baseParams)
        {
            var result = this.Container.Resolve<IBaseJurPersonContragentService>().AddContragents(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}