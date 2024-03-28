namespace Bars.Gkh.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    
    using B4;
    using B4.Utils;

    using Bars.Gkh.Entities.RealityObj;

    using Navigation;

    using Entities;
    using DomainService;
    using PassportProvider;
    using Domain;

    public class MenuController : BaseMenuController
    {
        public ActionResult GetContragentMenu(StoreLoadParams storeParams)
        {
            var id = storeParams.Params.GetAsId("objectId");
            if (id > 0)
                return new JsonNetResult(GetMenuItems("Contragent"));

            return new JsonNetResult(null);
        }

        public ActionResult GetManOrgMenu(StoreLoadParams storeParams)
        {
            var id = storeParams.Params.GetAsId("objectId");
            if (id > 0)
                return new JsonNetResult(GetMenuItems("ManOrg"));

            return new JsonNetResult(null);
        }

        public ActionResult GetPersonMenu(StoreLoadParams storeParams)
        {
            var id = storeParams.Params.GetAsId("objectId");
            if (id > 0)
            {
                InitActiveOperatorAndRoles();

                var domain = Container.Resolve<IDomainService<Person>>();

                try
                {
                    var person = domain.Get(id);

                    InitStatePermissions(person.State);

                    var menuItems = this.FilterInacessibleStateItems(GetMenuItems("Person"));

                    return new JsonNetResult(menuItems);
                }
                finally 
                {
                    Container.Release(domain);
                }
            }

            return new JsonNetResult(null);
        }

        public ActionResult GetManOrgLicenseMenu(StoreLoadParams storeParams)
        {
            var id = storeParams.Params.GetAs("objectId", 0L);
            var type = storeParams.Params.GetAs("type", "request");

            var servcie = Container.Resolve<IManOrgLicenseService>();
            try
            {
                if (id > 0)
                {
                    var result = servcie.GetInfo(type, id);

                    if (!result.Success)
                    {
                        return JsFailure(result.Message);
                    }

                    var info = (ManOrgLicenseInfo)result.Data;

                    var items = GetMenuItems("ManOrgLicense");

                    var resultList = new List<MenuItem>();

                    foreach (var item in items)
                    {
                        // если нет какого то идентификатора, то недобавляем такой пункт меню
                        if (item.Href == "manorglicense/{0}/{1}/editrequest" && info.requestId <= 0)
                        {
                            continue;
                        }

                        if (item.Href == "manorglicense/{0}/{1}/editlicense" && info.licenseId <= 0)
                        {
                            continue;
                        }

                        resultList.Add(item);
                    }

                    return new JsonNetResult(resultList);
                }

                return new JsonNetResult(null);
            }
            finally 
            {
                Container.Release(servcie);
            }

            
        }

        public ActionResult GetClaimWorkMenu(StoreLoadParams storeParams)
        {
            var id = storeParams.Params.GetAs("objectId", 0L);
            var type = storeParams.Params.GetAs("type", string.Empty);

            //изначально получаем пункт меню у конкретного основания
            var menuItems = GetMenuItems(type) ?? new List<MenuItem>();

            // потом получаем общие документы
            if (id > 0)
                 return new JsonNetResult(menuItems.Union(GetMenuItems("ClaimWork") ?? new List<MenuItem>()));

            return new JsonNetResult(menuItems);
        }

        public ActionResult GetServOrgMenu(StoreLoadParams storeParams)
        {
            var id = storeParams.Params.GetAsId("objectId");
            if (id > 0)
                return new JsonNetResult(GetMenuItems("ServOrg"));

            return new JsonNetResult(null);
        }

        public ActionResult GetRealityObjMenu(StoreLoadParams storeParams)
        {
            var id = storeParams.Params.GetAsId("objectId");
            if (id > 0)
            {
                this.InitActiveOperatorAndRoles();

                var realityObject = this.Container.Resolve<IDomainService<RealityObject>>().Get(id);

                this.InitStatePermissions(realityObject.State);

                var menuItems = this.GetMenuItems(RealityObjMenuKey.Key);

                var menuGkh = this.FilterInacessibleStateItems(menuItems);

                var allModificators = this.Container.ResolveAll<IMenuModificator>();
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
                    allModificators.ForEach(x => this.Container.Release(x));
                }

                if (realityObject.TypeHouse != Enums.TypeHouse.BlockedBuilding)
                {
                    menuGkh = menuGkh.Where(x => x.Caption != "Сведения о блоках");
                }

                var passport = this.Container.ResolveAll<IPassportProvider>().FirstOrDefault(x => x.Name == "Техпаспорт" && x.TypeDataSource == "xml");

                if (passport == null)
                {
                    return new JsonNetResult(menuGkh);
                }

                var menuTp = this.FilterInacessibleStateItems(passport.GetMenu());


                return new JsonNetResult(menuGkh.Union(menuTp));
            }

            return new JsonNetResult(null);
        }

        public ActionResult GetEmergencyObjMenu(StoreLoadParams storeParams)
        {
            var id = storeParams.Params.GetAsId("objectId");
            if (id > 0)
                return new JsonNetResult(GetMenuItems("EmergencyObj"));

            return new JsonNetResult(null);
        }

        public ActionResult GetBuilderMenu(StoreLoadParams storeParams)
        {
            var id = storeParams.Params.GetAsId("objectId");
            if (id > 0)
                return new JsonNetResult(GetMenuItems("Builder"));

            return new JsonNetResult(null);
        }

        public ActionResult GetSupplyResourceOrgMenu(StoreLoadParams storeParams)
        {
            var id = storeParams.Params.GetAsId("objectId");
            if (id > 0)
                return new JsonNetResult(GetMenuItems("SupplyResourceOrg"));

            return new JsonNetResult(null);
        }

        public ActionResult GetPublicServOrgMenu(StoreLoadParams storeParams)
        {
            var id = storeParams.Params.GetAsId("objectId");
            if (id > 0)
                return new JsonNetResult(GetMenuItems("PublicServOrg"));

            return new JsonNetResult(null);
        }

        public ActionResult GetLocalGovernmentMenu(StoreLoadParams storeParams)
        {
            var id = storeParams.Params.GetAsId("objectId");
            if (id > 0)
                return new JsonNetResult(GetMenuItems("LocalGovernment"));

            return new JsonNetResult(null);
        }

        public ActionResult GetPaymentAgentMenu(StoreLoadParams storeParams)
        {
            var id = storeParams.Params.GetAsId("objectId");
            if (id > 0)
                return new JsonNetResult(GetMenuItems("PaymentAgent"));

            return new JsonNetResult(null);
        }

        public ActionResult GetPoliticAuthorityMenu(StoreLoadParams storeParams)
        {
            var id = (storeParams.Params.GetAsId("objectId"));
            if (id > 0)
                return new JsonNetResult(GetMenuItems("PoliticAuthority"));

            return new JsonNetResult(null);
        }

        public ActionResult GetBelayOrgMenu(StoreLoadParams storeParams)
        {
            var id = (storeParams.Params.GetAsId("objectId"));
            if (id > 0)
                return new JsonNetResult(GetMenuItems("BelayOrg"));

            return new JsonNetResult(null);
        }

        public ActionResult GetBelayPolicyMenu(StoreLoadParams storeParams)
        {
            var id = (storeParams.Params.GetAsId("objectId"));
            if (id > 0)
                return new JsonNetResult(GetMenuItems("BelayPolicy"));

            return new JsonNetResult(null);
        }

        public ActionResult GetRegOperatorMenu(StoreLoadParams storeParams)
        {
            var id = storeParams.Params.GetAsId("objectId");
            if (id > 0)
                return new JsonNetResult(GetMenuItems("RegOperator"));
            return new JsonNetResult(null);
        }

        public ActionResult GetGkuInfoMenu(StoreLoadParams storeParams)
        {
            var id = storeParams.Params.GetAsId("objectId");
            if (id > 0)
                return new JsonNetResult(GetMenuItems("GkuInfo"));

            return new JsonNetResult(null);
        }

        public ActionResult GetWorksCrMenu(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId("objectId");
            if (id > 0L)
            {
                return new JsonNetResult(GetMenuItems("WorksCr"));
            }

            return JsonNetResult.Success;
        }

        public ActionResult GetHousingInspectionMenu(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId("objectId");
            if (id > 0L)
            {
                return new JsonNetResult(this.GetMenuItems("HousingInspection"));
            }

            return JsonNetResult.Success;
        }

        public ActionResult GetGasEquipmentOrgMenu(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId("objectId");
            if (id > 0L)
            {
                return new JsonNetResult(this.GetMenuItems("GasEquipmentOrg"));
            }

            return JsonNetResult.Success;
        }

        /// <summary>
        /// Получает меню для "Двор".
        /// </summary>
        /// <param name="baseParams">Базовые параметры.</param>
        /// <returns></returns>
        public ActionResult GetOutdoorMenu(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId("objectId");
            return id > 0
                ? new JsonNetResult(this.GetMenuItems(nameof(RealityObjectOutdoor)))
                : new JsonNetResult(null);
        }

        public ActionResult GetContragentClwMenu(StoreLoadParams storeParams)
        {
            var id = storeParams.Params.GetAs<long>("objectId");
            if (id > 0)
                return new JsonNetResult(GetMenuItems("ContragentClw"));

            return new JsonNetResult(null);
        }
    }
}
