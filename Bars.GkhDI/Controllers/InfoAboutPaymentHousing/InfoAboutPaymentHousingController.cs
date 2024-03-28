namespace Bars.GkhDi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using DomainService;
    using Entities;

    public class InfoAboutPaymentHousingController : B4.Alt.DataController<InfoAboutPaymentHousing>
    {
        public ActionResult SaveInfoAboutPaymentHousing(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IInfoAboutPaymentHousingService>().SaveInfoAboutPaymentHousing(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }

    }
}

