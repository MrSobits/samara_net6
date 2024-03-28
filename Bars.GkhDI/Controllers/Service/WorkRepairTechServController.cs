namespace Bars.GkhDi.Controllers
{
    using System.Collections;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.GkhDi.DomainService;
    using Bars.GkhDi.Entities;

    public class WorkRepairTechServController : B4.Alt.DataController<WorkRepairTechServ>
    {
        public ActionResult AddWorks(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IWorkRepairTechServService>().AddWorks(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult SaveRepairService(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IWorkRepairTechServService>().SaveRepairService(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult ListTree(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IWorkRepairTechServService>().ListTree(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult ListSelected(BaseParams baseParams)
        {
            var result = (ListDataResult)Resolve<IWorkRepairTechServService>().ListSelected(baseParams);
            return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonListResult.Failure(result.Message);
        }
    }
}
