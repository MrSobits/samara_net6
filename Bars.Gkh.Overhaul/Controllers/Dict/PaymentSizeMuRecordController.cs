namespace Bars.Gkh.Overhaul.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using B4;
    using DomainService;
    using Entities;

    public class PaymentSizeMuRecordController : B4.Alt.DataController<PaymentSizeMuRecord>
    {
        public ActionResult AddMuRecords(BaseParams baseParams)
        {
            var result = Container.Resolve<IPaymentSizeMuRecordService>().AddMuRecords(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}