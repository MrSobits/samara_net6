namespace Bars.Gkh.Overhaul.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Controllers;
    using Bars.Gkh.Overhaul.DomainService;

    public class OvrhlWorkController : WorkController
    {
        public override ActionResult Create(BaseParams baseParams) 
        {
            var result = Resolve<IWorkService>().SaveWithFinanceType(baseParams, DomainService);
            return new JsonNetResult(result);
        }

        public override ActionResult Update(BaseParams baseParams)
        {
            var result = Resolve<IWorkService>().UpdateWithFinanceType(baseParams, DomainService);
            return new JsonNetResult(result);
        }

        public override ActionResult Delete(BaseParams baseParams)
        {
            var result = Resolve<IWorkService>().DeleteWithFinanceType(baseParams, DomainService);
            return new JsonNetResult(result);
        }
    }
}