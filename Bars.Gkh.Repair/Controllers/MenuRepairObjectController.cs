namespace Bars.Gkh.Repair.Controllers
{
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Controllers;
    using Bars.Gkh.Repair.Entities;

    using Microsoft.AspNetCore.Mvc;

    public class MenuRepairObjectController : BaseMenuController
    {
        public IDomainService<RepairObject> RepairObjectDomainService { get; set; }

        public ActionResult GetRepairObjectMenu(StoreLoadParams storeParams)
        {
            var id = storeParams.Params.ContainsKey("objectId") ? storeParams.Params["objectId"].ToLong() : 0;
            if (id > 0)
            {
                InitActiveOperatorAndRoles();

                var repairObject = RepairObjectDomainService.Get(id);

                InitStatePermissions(repairObject.State);

                var menuItems = this.FilterInacessibleStateItems(GetMenuItems("RepairObject"));

                return new JsonNetResult(menuItems);
            }

            return new JsonNetResult(null);
        }
    }
}
