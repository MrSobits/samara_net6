namespace Bars.Gkh.Gis.Controllers.WasteCollection
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using Gkh.Controllers;

    public class MenuWasteCollectionController : BaseMenuController
    {
        public ActionResult GetWasteCollectionMenu(StoreLoadParams storeParams)
        {
            var id = storeParams.Params.GetAs<long>("objectId");
            if (id > 0)
                return new JsonNetResult(GetMenuItems("WasteCollection"));

            return new JsonNetResult(null);
        }
    }
}