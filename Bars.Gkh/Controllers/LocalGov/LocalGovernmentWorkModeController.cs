namespace Bars.Gkh.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;

    public class LocalGovernmentWorkModeController : B4.Alt.DataController<LocalGovernmentWorkMode>
    {
        public ActionResult AddWorkMode(BaseParams baseParams)
        {
            var result = this.Container.Resolve<ILocalGovernmentWorkModeService>().AddWorkMode(baseParams);

            if (result.Success)
            {
                return new JsonNetResult(new { success = true });
            }

            return JsonNetResult.Failure(result.Message);
        }
    }
}
