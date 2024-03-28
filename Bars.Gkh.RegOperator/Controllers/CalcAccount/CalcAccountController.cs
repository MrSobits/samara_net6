namespace Bars.Gkh.RegOperator.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using B4.IoC;
    using DomainModelServices;
    using DomainService;
    using Entities;

    public class CalcAccountController : B4.Alt.DataController<CalcAccount>
    {
        public ActionResult GetRegopAccountSummary(BaseParams baseParams)
        {
            var service = Container.Resolve<ICalcAccountService>();
            using (Container.Using(service))
            {
                return new JsonNetResult(service.GetRegopAccountSummary(baseParams));
            }
        }

        public ActionResult ListOperations(BaseParams baseParams)
        {
            var service = Container.Resolve<ICalcAccountMoneyService>();
            using (Container.Using(service))
            {
                return new JsonNetResult(service.List(baseParams));
            }
        }

        public ActionResult ListOperationsSum(BaseParams baseParams)
        {
            var service = Container.Resolve<ICalcAccountService>();
            using (Container.Using(service))
            {
                return new JsonNetResult(service.ListOperationsSum(baseParams));
            }
        }
    }
}