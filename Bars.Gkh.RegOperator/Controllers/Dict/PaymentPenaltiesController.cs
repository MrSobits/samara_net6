namespace Bars.Gkh.RegOperator.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using DomainService;
    using Entities.Dict;

    public class PaymentPenaltiesController : B4.Alt.DataController<PaymentPenalties>
    {
        public IPaymentPenaltiesService Service { get; set; }

        public ActionResult AddExcludePersAccs(BaseParams baseParams)
        {
            var result = Service.AddExcludePersAccs(baseParams);
            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        }
    }
}