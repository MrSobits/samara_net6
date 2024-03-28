namespace Bars.Gkh.Overhaul.Nso.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Controllers;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Nso.Enum;
    using Bars.Gkh.PassportProvider;
    using Entities;
    using Gkh.Entities;


    public class RealityObjMenuController : BaseMenuController
    {
        public IDomainService<BasePropertyOwnerDecision> BasePropOwnDecisionDomain { get; set; }
        public IDomainService<RealityObject> RealityObjDomain { get; set; }

        public ActionResult GetRealityObjMenu(StoreLoadParams storeParams)
        {
            var id = storeParams.Params.GetAs<long>("objectId");

            if (id > 0)
            {
                InitActiveOperatorAndRoles();
                
                var realityObject = RealityObjDomain.Get(id);

                InitStatePermissions(realityObject.State);
                var menuItems = FilterInacessibleStateItems(GetMenuItems("RealityObj"));
                var menuGkh = GetMenuItemByDecision(menuItems, realityObject);

                var passport = Container.ResolveAll<IPassportProvider>().FirstOrDefault(x => x.Name == "Техпаспорт" && x.TypeDataSource == "xml");

                if (passport == null)
                {
                    return new JsonNetResult(menuGkh);
                }

                var menuTp = FilterInacessibleStateItems(passport.GetMenu());

                return new JsonNetResult(menuGkh.Union(menuTp));
            }

            return new JsonNetResult(null);
        }

        private IEnumerable<MenuItem> GetMenuItemByDecision(IEnumerable<MenuItem> menuItems, RealityObject realObj)
        {
            var condition = (realObj.TypeHouse == TypeHouse.ManyApartments
                             || realObj.TypeHouse == TypeHouse.BlockedBuilding
                             || realObj.TypeHouse == TypeHouse.SocialBehavior)
                            && (realObj.ConditionHouse == ConditionHouse.Dilapidated
                                || realObj.ConditionHouse == ConditionHouse.Serviceable);

            var methodFormFunds =
                BasePropOwnDecisionDomain.GetAll()
                    .Where(x => x.RealityObject.Id == realObj.Id)
                    .Where(x => x.PropertyOwnerDecisionType == PropertyOwnerDecisionType.SelectMethodForming)
                    .Select(x => x.MethodFormFund)
                    .ToArray();

            var noAvailibleControllers = new List<string>();

            if (!condition)
            {
                noAvailibleControllers.Add("B4.controller.realityobj.OwnerProtocol");
            }
            
            if (!condition || !methodFormFunds.Contains(MethodFormFundCr.SpecialAccount))
            {
                noAvailibleControllers.Add("B4.controller.longtermprobject.SpecialAccount");
            }

            if (!methodFormFunds.Contains(MethodFormFundCr.RegOperAccount))
            {
                noAvailibleControllers.Add("B4.controller.longtermprobject.PaymentAccount");
            }

            if (!methodFormFunds.Any())
            {
                noAvailibleControllers.Add("B4.controller.longtermprobject.AccrualsAccount");
            }

            return FilterByNoAvialableConrollers(menuItems, noAvailibleControllers);
        }
    }
}
