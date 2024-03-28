namespace Bars.Gkh.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;

    public class PoliticAuthorityWorkModeController : B4.Alt.DataController<PoliticAuthorityWorkMode>
    {
        public ActionResult AddWorkMode(BaseParams baseParams)
        {
            var result = Container.Resolve<IPoliticAuthorityWorkModeService>().AddWorkMode(baseParams);
            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        }
    }
}