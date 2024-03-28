namespace Bars.Gkh.Overhaul.Nso.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Nso.DomainService;
    using Bars.Gkh.Overhaul.Nso.Entities;

    public class SubsidyRecordController : B4.Alt.DataController<SubsidyRecord>
    {
        public ISubsidyRecordService Service { get; set; }

        public ActionResult GetSubsidy(BaseParams baseParams)
        {
            return new JsonNetResult(Service.GetSubsidy(baseParams));
        }

        public ActionResult CalcOwnerCollection(BaseParams baseParams)
        {
            return new JsonNetResult(Service.CalcOwnerCollection(baseParams));
        }

        public ActionResult CalcValues(BaseParams baseParams)
        {
            return new JsonNetResult(Service.CalcValues(baseParams));
        }

        public ActionResult Export(BaseParams baseParams)
        {
            return new ReportStreamResult(Service.PrintReport(baseParams), "subsidy_export.xls");
        }
    }
}
