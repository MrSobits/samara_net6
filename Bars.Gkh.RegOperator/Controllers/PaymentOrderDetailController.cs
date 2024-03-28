namespace Bars.Gkh.RegOperator.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using DomainService;
    using Entities;
    using Gkh.Domain;

    public class PaymentOrderDetailController : B4.Alt.BaseDataController<PaymentOrderDetail>
    {
        public IPerformedWorkActPaymentService Service { get; set; }

        public ActionResult BatchSaveWithOrder(BaseParams baseParams)
        {
            return Service.SaveWithDetails(baseParams).ToJsonResult();
        }
    }
}