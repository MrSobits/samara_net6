namespace Bars.Gkh.Overhaul.Hmao.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Hmao.DomainService;
    using Bars.Gkh.Overhaul.Hmao.Entities.PriorityParams;

    public class PriorityParamAdditionController : B4.Alt.DataController<PriorityParamAddition>
    {
        public ActionResult GetValue(BaseParams baseParams)
        {
            var result = Container.Resolve<IPriorityParamAdditionService>().GetValue(baseParams);
            return new JsonNetResult(result.Data);
        }
    }
}