namespace Bars.GkhDi.Controllers
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.GkhDi.DomainService;
    using Bars.GkhDi.Entities;

    public class WorkRepairListController : B4.Alt.DataController<WorkRepairList>
    {
        public ActionResult ListSelected(BaseParams baseParams)
        {
            var result = (ListDataResult)Resolve<IWorkRepairListService>().ListSelected(baseParams);
            return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonListResult.Failure(result.Message);
        }

        public ActionResult SaveWorkPpr(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IWorkRepairListService>().SaveWorkPpr(baseParams);
            return result.Success ? new JsonNetResult(new { succes = true }) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult HasDetailAllWorkRepair(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IWorkRepairListService>().HasDetailAllWorkRepair(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}
