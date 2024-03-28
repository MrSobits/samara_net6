namespace Bars.Gkh.Overhaul.Tat.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using B4;


    using DomainService;
    using Gkh.Entities.RealEstateType;

    public class RealEstateTypeRateController : B4.Alt.DataController<RealEstateTypeRate>
    {
        public ActionResult CalculateRates(BaseParams baseParams)
        {
            var result = (BaseDataResult)Container.Resolve<IRealEstateTypeRateService>().CalculateRates(baseParams);
            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        } 
    }
}