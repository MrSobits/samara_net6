namespace Bars.Gkh.Repair.Controllers
{
    using Bars.B4;
    using Bars.Gkh.Repair.DomainService;
    using Bars.Gkh.Repair.Entities.RepairControlDate;

    using Microsoft.AspNetCore.Mvc;

    public class RepairControlDateController : B4.Alt.DataController<RepairControlDate>
    {
        public IRepairControlDateService RepairControlDateService { get; set; }

        public ActionResult AddWorks(BaseParams baseParams)
        {
            var result = (BaseDataResult)RepairControlDateService.AddWorks(baseParams);
            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        }
    }
}
