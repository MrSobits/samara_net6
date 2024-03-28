namespace Bars.GkhRf.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.GkhRf.DomainService;
    using Bars.GkhRf.Entities;

    public class TransferFundsRfController : B4.Alt.DataController<TransferFundsRf>
    {
        public ActionResult AddTransferFundsObjects(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<ITransferFundsRfService>().AddTransferFundsObjects(baseParams);
            return result.Success ? new JsonNetResult(new {success = true}) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult ListPersonalAccount(BaseParams baseParams)
        {
            var result = (ListDataResult)Resolve<ITransferFundsRfService>().ListPersonalAccount(baseParams);
            return new JsonNetResult(new { success = true, data = result.Data, totalCount = result.TotalCount });
        }
    }
}