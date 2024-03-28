namespace Bars.Gkh.RegOperator.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Controllers;

    public class RegOperatorMenuController : BaseMenuController
    {
        public ActionResult GetDeliveryAgentMenu(StoreLoadParams storeParams)
        {
            var id = storeParams.Params.GetAs<long>("objectId");
            if (id > 0)
                return new JsonNetResult(GetMenuItems("DeliveryAgent"));

            return new JsonNetResult(null);
        }

        public ActionResult GetCashPaymentCenterMenu(StoreLoadParams storeParams)
        {
            var id = storeParams.Params.GetAs<long>("objectId");
            if (id > 0)
                return new JsonNetResult(GetMenuItems("CashPaymentCenter"));

            return new JsonNetResult(null);
        }
    }
}
