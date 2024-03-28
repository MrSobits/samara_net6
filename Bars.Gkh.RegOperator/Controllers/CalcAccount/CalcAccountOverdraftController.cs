namespace Bars.Gkh.RegOperator.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using Bars.B4.IoC;
    using DomainService;
    using Entities;

    public class CalcAccountOverdraftController : B4.Alt.DataController<CalcAccountOverdraft>
    {
        public ActionResult GetRobjectOverdraft(BaseParams baseParams)
        {
            var service = Container.Resolve<ICalcAccountOverdraftService>();

            using (Container.Using(service))
            {
                return new JsonNetResult(new {Sum = service.GetRobjectOverdraft(baseParams)});
            }
        }
    }
}