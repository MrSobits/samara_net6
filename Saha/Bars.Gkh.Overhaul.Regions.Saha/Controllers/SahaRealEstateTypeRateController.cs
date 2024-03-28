namespace Bars.Gkh.Overhaul.Regions.Saha.Controllers
{
    using Bars.B4;
    using Bars.Gkh.Overhaul.Hmao.DomainService;
    using Bars.Gkh.Overhaul.Regions.Saha.Entities.RealEstateType;

    using Microsoft.AspNetCore.Mvc;

    public class SahaRealEstateTypeRateController : B4.Alt.DataController<SahaRealEstateTypeRate>
    {
        public ActionResult CalculateRates(BaseParams baseParams)
        {
            var result = (BaseDataResult)Container.Resolve<IRealEstateTypeRateService>().CalculateRates(baseParams);
            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        }
    }
}