namespace Bars.Gkh.Overhaul.Tat.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using DomainService;

    public class SubsidyMunicipalityController : B4.Alt.DataController<SubsidyMunicipality>
    {
        public ISubsidyMunicipalityService Service { get; set; }

        public ActionResult GetSubsidy(BaseParams baseParams)
        {
            var result = Service.GetSubsidy(baseParams);
            return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult CalcValues(BaseParams baseParams)
        {
            var result = Service.CalcValues(baseParams);
            return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult CorrectDpkr(BaseParams baseParams)
        {
            var data = Service.CorrectDpkr(baseParams);
            return data.Success ? new JsonNetResult(data) : JsonNetResult.Failure(data.Message);
        }
    }
}
