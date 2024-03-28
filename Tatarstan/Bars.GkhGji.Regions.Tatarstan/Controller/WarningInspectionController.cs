namespace Bars.GkhGji.Regions.Tatarstan.Controller
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Controllers;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tatarstan.DomainService;

    public class WarningInspectionController : WarningInspectionController<WarningInspection>
    {
        public IWarningInspectionService Service { get; set; }

        public ActionResult CheckAppealCits(BaseParams baseParams)
        {
            return this.Service.CheckAppealCits(baseParams).ToJsonResult();
        }

        public ActionResult CreateWithAppealCits(BaseParams baseParams)
        {
            var result = this.Service.CreateWithAppealCits(baseParams);

            return result.Success
                ? new JsonNetResult(new { success = true, message = result.Message, data = result.Data })
                {
                    ContentType = "text/html; charset=utf-8"
                }
                : JsonNetResult.Failure(result.Message);
        }

        public ActionResult ListByAppealCits(BaseParams baseParams)
        {
            return this.Service.ListByAppealCits(baseParams).ToJsonResult();
        }
    }
}