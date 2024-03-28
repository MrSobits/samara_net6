namespace Bars.Gkh.Overhaul.Hmao.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Controllers;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Navigation;
    using Bars.Gkh.Overhaul.Hmao.Enum;
    using Bars.Gkh.PassportProvider;

    using Entities;

    public class RealityObjMenuController : BaseMenuController
    {
        public IDomainService<BasePropertyOwnerDecision> BasePropOwnDecisionDomain { get; set; }

        public ActionResult GetRealityObjMenu(StoreLoadParams storeParams)
        {
            var id = storeParams.Params.GetAs<long>("objectId");

            if (id > 0)
            {
                InitActiveOperatorAndRoles();

                var realityObject = Container.Resolve<IDomainService<RealityObject>>().Get(id);

                InitStatePermissions(realityObject.State);

                var menuItems = this.FilterInacessibleStateItems(GetMenuItems("RealityObj"));

                var menuGkh = GetMenuItemByDecision(menuItems, realityObject);

                var allModificators = Container.ResolveAll<IMenuModificator>();
                try
                {
                    var modificators = allModificators.Where(x => string.IsNullOrWhiteSpace(x.Key) || x.Key == RealityObjMenuKey.Key);

                    foreach (var modificator in modificators)
                    {
                        menuGkh = modificator.Modify(menuGkh);
                    }
                }
                finally
                {
                    allModificators.ForEach(x => Container.Release(x));
                }

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

        private IEnumerable<MenuItem> GetMenuItemByDecision(IEnumerable<MenuItem> menuItems, RealityObject realityObject)
        {
            var condition = realityObject.TypeHouse == TypeHouse.ManyApartments
                            && (realityObject.ConditionHouse == ConditionHouse.Dilapidated
                                || realityObject.ConditionHouse == ConditionHouse.Serviceable);

            var methodFormFunds =
                BasePropOwnDecisionDomain.GetAll()
                    .Where(x => x.RealityObject.Id == realityObject.Id)
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

            if (!condition || !methodFormFunds.Contains(MethodFormFundCr.RegOperAccount))
            {
                noAvailibleControllers.Add("B4.controller.longtermprobject.PaymentAccount");
            }

            if (!condition || !methodFormFunds.Any())
            {
                noAvailibleControllers.Add("B4.controller.longtermprobject.AccrualsAccount");
            }

            return FilterByNoAvialableConrollers(menuItems, noAvailibleControllers);
        }
    }
}
