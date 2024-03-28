namespace Bars.Gkh.RegOperator.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using B4;

    using Bars.Gkh.Repositories.ChargePeriod;

    using Domain.Repository;
    using DomainService;
    using Entities;

    public class SpecialCalcAccountController : B4.Alt.DataController<SpecialCalcAccount>
    {
        public ISpecialCalcAccountService SpecialCalcAccountService { get; set; }

        public ActionResult ListRegister(BaseParams baseParams)
        {
            return new JsonNetResult(SpecialCalcAccountService.ListRegister(baseParams));
        }

        public ActionResult EditPaymentCrSpecAccNotRegop(BaseParams baseParams)
        {
            return new JsonNetResult(SpecialCalcAccountService.EditPaymentCrSpecAccNotRegop(baseParams));
        }

        public ActionResult GetPaymentCrSpecAccNotRegop(BaseParams baseParams)
        {
            return new JsonNetResult(SpecialCalcAccountService.GetPaymentCrSpecAccNotRegop(baseParams));
        }

        public ActionResult ListPaymentCrSpecAccNotRegop(BaseParams baseParams)
        {
            return new JsonNetResult(SpecialCalcAccountService.ListPaymentCrSpecAccNotRegop(baseParams));
        }

        public ActionResult GetCurrentPeriod()
        {
            return new JsonNetResult(Resolve<IChargePeriodRepository>().GetCurrentPeriod());
        }
    }
}