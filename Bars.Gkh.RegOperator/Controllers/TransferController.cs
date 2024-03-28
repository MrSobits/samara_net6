namespace Bars.Gkh.RegOperator.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using DomainService;

    public class TransferController : B4.Alt.DataController<Transfer>
    {
        public ITransferService Service { get; set; }

        public ActionResult ListTransferForPaymentAccount(BaseParams baseParams)
        {
            return new JsonNetResult(Service.ListTransferForPaymentAccount(baseParams));
        }

        public ActionResult ListTransferForSubsidyAccount(BaseParams baseParams)
        {
            return new JsonNetResult(Service.ListTransferForSubsidyAccount(baseParams));
        }
    }
}