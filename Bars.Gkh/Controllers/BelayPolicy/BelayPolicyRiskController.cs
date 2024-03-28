namespace Bars.Gkh.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;

    public class BelayPolicyRiskController : B4.Alt.DataController<BelayPolicyRisk>
    {
        public ActionResult AddKindRisk(BaseParams baseParams)
        {
            var result = Container.Resolve<IBelayPolicyRiskService>().AddKindRisk(baseParams);
            return new JsonNetResult(new { success = result.Success, message = result.Message });
        }
    }
}