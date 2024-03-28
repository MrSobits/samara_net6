namespace Bars.Gkh.RegOperator.Controllers.Period
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.RegOperator.DomainService.Period;
    using Bars.Gkh.RegOperator.Entities.Period;

    public class PeriodCloseCheckController : B4.Alt.DataController<PeriodCloseCheck>
    {
        public ActionResult ListCheckers(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IPeriodCloseCheckService>();
            try
            {
                return new JsonNetResult(service.ListCheckers(baseParams));
            }
            finally
            {
                this.Container.Release(service);
            }
        }
    }
}