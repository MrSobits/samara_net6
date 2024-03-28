namespace Bars.Gkh.RegOperator.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using B4;

    using Bars.Gkh.RegOperator.DomainService;
    using Bars.GkhCr.Entities;

    public class PaymentSrcFinanceDetailsController : B4.Alt.DataController<PaymentSrcFinanceDetails>
    {
        public IPaymentSrcFinanceDetailsService Service { get; set; }
        
        public ActionResult SaveRecords(BaseParams baseParams)
        {
            var result = Service.SaveRecords(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : this.JsFailure(result.Message);
        }
        
    }
}