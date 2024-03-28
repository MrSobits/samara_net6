namespace Bars.Gkh.RegOperator.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using DomainService;
    using Entities;

    public class TransferObjectController : B4.Alt.DataController<TransferObject>
    {
        public ActionResult Calc(BaseParams baseParams)
        {
            var result = Resolve<ITransferObjectService>().Calc(baseParams);
            return result.Success ? JsSuccess() : JsFailure(result.Message);
        }
    }
}