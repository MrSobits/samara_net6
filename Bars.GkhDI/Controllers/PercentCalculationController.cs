namespace Bars.GkhDi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using B4;

    using Bars.GkhDi.DomainService;

    public class PercentCalculationController : BaseController
    {

        public virtual IDisclosureInfoService DisclosureInfoService { get; set; }

        public ActionResult MassCalculate(BaseParams baseParams)
        {
            var result = DisclosureInfoService.PercentCalculation(baseParams); 

            return result.Success ? new JsonNetResult(result.Data) : this.JsFailure(result.Message);
        }
    }
}
