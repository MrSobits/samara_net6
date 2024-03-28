namespace Bars.Gkh.Overhaul.Nso.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Nso.DomainService;
    using Bars.Gkh.Overhaul.Nso.Entities;

    public class SubsidyMunicipalityController : B4.Alt.DataController<SubsidyMunicipality>
    {
        public ISubsidyMunicipalityService Service { get; set; }

        public ActionResult GetSubsidy(BaseParams baseParams)
        {
            return new JsonNetResult(Service.GetSubsidy(baseParams));
        }

        public ActionResult CalcFinanceNeedBefore(BaseParams baseParams)
        {
            return new JsonNetResult(Service.CalcFinanceNeedBefore(baseParams));
        }

        public ActionResult CalcValues(BaseParams baseParams)
        {
            return new JsonNetResult(Service.CalcValues(baseParams));
        }

        public ActionResult CorrectDpkr(BaseParams baseParams)
        {
            var data = Service.CorrectDpkr(baseParams);
            return data.Success ? new JsonNetResult(data) : JsonNetResult.Failure(data.Message);
        }

        public ActionResult CheckCalculation(BaseParams baseParams)
        {
            return new JsonNetResult(new {success = Service.CheckCalculation(baseParams)});
        }

        public ActionResult Export(BaseParams baseParams)
        {
            return new ReportStreamResult(Service.PrintReport(baseParams), "subsidy_export.xls");
        }
    }
}
