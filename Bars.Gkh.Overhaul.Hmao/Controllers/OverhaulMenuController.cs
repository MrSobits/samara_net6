namespace Bars.Gkh.Overhaul.Hmao.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Controllers;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Entities;

    public class OverhaulMenuController : BaseMenuController
    {
        public ActionResult GetLongTermPrObjectMenu(StoreLoadParams storeParams)
        {
            var id = storeParams.Params.GetAs<long>("objectId");

            if (id > 0)
            {
                var obj =
                    Container.Resolve<IDomainService<LongTermPrObject>>().GetAll()
                        .Where(x => x.Id == id)
                        .Select(x => x.RealityObject.MethodFormFundCr)
                        .FirstOrDefault();

                if (obj == MethodFormFundCr.RegOperAccount)
                {
                    return new JsonNetResult(GetMenuItems("LongTermPrObjectReal"));
                }

                if (obj == MethodFormFundCr.SpecialAccount)
                {
                    return new JsonNetResult(GetMenuItems("LongTermPrObjectSpecial"));
                }

                return new JsonNetResult(GetMenuItems("LongTermPrObject"));
            }

            return new JsonNetResult(null);
        }
    }
}
