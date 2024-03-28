namespace Bars.GkhGji.Regions.Tomsk.Controller
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.Regions.Tomsk.DomainService.ActCheck;

    public class ActCheckTimeController : BaseController
    {
        public IActCheckTimeService Service { get; set; }

        public ActionResult CreateActCheckTime(BaseParams baseParams)
        {
            var result = Service.CreateActCheckTime(baseParams);

            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        } 
    }
}