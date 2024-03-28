namespace Bars.Gkh.Overhaul.Nso.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Nso.DomainService;
    using Bars.Gkh.Overhaul.Nso.Entities;

    public class PriorityParamAdditionController : B4.Alt.DataController<PriorityParamAddition>
    {
        public ActionResult GetValue(BaseParams baseParams)
        {
            var result = Container.Resolve<IPriorityParamAdditionService>().GetValue(baseParams);
            return new JsonNetResult(result.Data);
        }
    }
}