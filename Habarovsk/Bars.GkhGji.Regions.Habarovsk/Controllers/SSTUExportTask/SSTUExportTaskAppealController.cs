namespace Bars.GkhGji.Regions.Habarovsk.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using System.Collections;
    using Bars.GkhGji.Regions.Habarovsk.Entities;
    using Bars.GkhGji.Regions.Habarovsk.DomainService;

    public class SSTUExportTaskAppealController : B4.Alt.DataController<SSTUExportTaskAppeal>
    {
        public ISSTUExportTaskAppealService service { get; set; }
        public ActionResult AddAppeal(BaseParams baseParams)
        {
            var result = service.AddAppeal(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult GetListExportableAppeals(BaseParams baseParams)
        {
            int totalCount;
            var result = service.GetListExportableAppeals(baseParams, true, out totalCount);

            return result.Success ? new JsonListResult((IList)result.Data, totalCount) : JsFailure(result.Message);
        }
    }
}