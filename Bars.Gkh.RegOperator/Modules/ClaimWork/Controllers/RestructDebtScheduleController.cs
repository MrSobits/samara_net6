namespace Bars.Gkh.ClaimWork.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.ClaimWork.Entities;
    using Bars.Gkh.Domain;
    using Bars.Gkh.RegOperator.Modules.ClaimWork.DomainService;

    public class RestructDebtScheduleController : B4.Alt.BaseDataController<RestructDebtSchedule>
    {
        public IRestructDebtScheduleService Service { get; set; }

        public ActionResult CreateRestructSchedule(BaseParams baseParams)
        {
            return this.Service.CreateRestructSchedule(baseParams).ToJsonResult();
        }

        public ActionResult ListAccountInfo(BaseParams baseParams)
        {
            return this.Service.ListAccountInfo(baseParams).ToJsonResult();
        }
    }
}