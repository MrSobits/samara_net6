namespace Bars.Gkh.RegOperator.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using Bars.B4.IoC;
    using Bars.Gkh.RegOperator.Entities;
    using DomainService.RealityObjectAccount;

    public class RealityObjectSubsidyAccountController : B4.Alt.DataController<RealityObjectSubsidyAccount>
    {
        public ActionResult GetPlanSubsidyOperations(BaseParams baseParams)
        {
            var subsidyService = Container.Resolve<IRealityObjectSubsidyService>();

            using (Container.Using(subsidyService))
            {
                return new JsonNetResult(subsidyService.GetPlanSubsidyOperations(baseParams));
            }
        }
    }
}