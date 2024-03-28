namespace Bars.GkhRf.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Controllers;

    public class MenuRegFundController : MenuController
    {
        public ActionResult GetPaymentMenu(StoreLoadParams storeParams)
        {
            var id = storeParams.Params.ContainsKey("objectId") ? storeParams.Params["objectId"].ToLong() : 0;
            if (id > 0)
            {
                return new JsonNetResult(this.GetMenuItems("Payment"));
            }

            return new JsonNetResult(null);
         }
    }
}
