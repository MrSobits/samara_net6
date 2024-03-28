namespace Bars.Gkh.Overhaul.Nso.Controllers
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Nso.DomainService;
    using Bars.Gkh.Overhaul.Nso.Entities;

    public class OwnerAccountDecisionController : B4.Alt.DataController<OwnerAccountDecision>
    {
        public ActionResult ListContragents(BaseParams baseParams)
        {
            var result = (ListDataResult)Container.Resolve<IOwnerAccountDecisionService>().ListContragents(baseParams);
            return new JsonListResult((IEnumerable)result.Data, result.TotalCount);
        }
    }
}