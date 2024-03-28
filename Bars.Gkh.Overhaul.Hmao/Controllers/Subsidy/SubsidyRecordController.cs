namespace Bars.Gkh.Overhaul.Hmao.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using DomainService;

    public class SubsidyRecordController : B4.Alt.DataController<SubsidyRecord>
    {
        public ISubsidyRecordService Service { get; set; }

        public ActionResult GetSubsidy(BaseParams baseParams)
        {
            var result = Service.GetSubsidy(baseParams);
            return result.Success ? JsSuccess(result) : JsFailure(result.Message);
        }

        public ActionResult CalcOwnerCollection(BaseParams baseParams)
        {
            return new JsonNetResult(Service.CalcOwnerCollection(baseParams));
        }
        public ActionResult UpdateSaldoBallance(BaseParams baseParams)
        {
            return new JsonNetResult(Service.UpdateSaldoBallance(baseParams));
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