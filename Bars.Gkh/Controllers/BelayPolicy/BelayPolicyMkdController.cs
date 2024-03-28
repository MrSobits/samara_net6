namespace Bars.Gkh.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;

    public class BelayPolicyMkdController : B4.Alt.DataController<BelayPolicyMkd>
    {
        public ActionResult AddPolicyMkdObjects(BaseParams baseParams)
        {
            var result = Container.Resolve<IBelayPolicyMkdService>().AddPolicyMkdObjects(baseParams);
            return new JsonNetResult(new {success = result.Success, message = result.Message});
        }
    }
}