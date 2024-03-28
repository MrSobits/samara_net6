namespace Bars.GkhGji.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class DisposalExpertController : B4.Alt.DataController<DisposalExpert>
    {
        public ActionResult AddExperts(BaseParams baseParams)
        {
            var result = Container.Resolve<IDisposalExpertService>().AddExperts(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}