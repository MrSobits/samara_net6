namespace Bars.GkhDi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using DomainService;
    using Entities;

    public class InfoAboutReductionPaymentController : B4.Alt.DataController<InfoAboutReductionPayment>
    {
        public ActionResult AddTemplateService(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IInfoAboutReductionPaymentService>().AddTemplateService(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}

