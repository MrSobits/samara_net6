namespace Bars.Gkh.Overhaul.Tat.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Controllers;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Entities;

    public class OverhaulMenuController : BaseMenuController
    {
        public ActionResult GetLongTermPrObjectMenu(StoreLoadParams storeParams)
        {
            var id = (storeParams.Params.ContainsKey("objectId")) ? storeParams.Params["objectId"].ToLong() : 0;
            if (id > 0)
            {
                var obj = Container.Resolve<IDomainService<LongTermPrObject>>().GetAll().FirstOrDefault(x => x.Id == id);

                if (obj != null)
                {
                    if (obj.RealityObject.MethodFormFundCr == MethodFormFundCr.RegOperAccount)
                    {
                        return new JsonNetResult(GetMenuItems("LongTermPrObjectReal"));
                    }

                    if (obj.RealityObject.MethodFormFundCr == MethodFormFundCr.SpecialAccount)
                    {
                        return new JsonNetResult(GetMenuItems("LongTermPrObjectSpecial"));
                    }
                }

                return new JsonNetResult(GetMenuItems("LongTermPrObject"));
            }

            return new JsonNetResult(null);
        }
    }
}
