using Bars.B4;

namespace Bars.Gkh1468
{
    public class ClientRouteMapRegistrar : IClientRouteMapRegistrar
    {
        public void RegisterRoutes(B4.ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("passpstructs/", "B4.controller.PassportStruct", requiredPermission: "Gkh1468.Dictionaries.PassportStruct.View"));
            map.AddRoute(new ClientRoute("structeditor/new/", "B4.controller.PassportStruct", "structeditor", requiredPermission: "Gkh1468.Dictionaries.PassportStruct.Create"));
            map.AddRoute(new ClientRoute("structeditor/{id}/", "B4.controller.PassportStruct", "structeditor", requiredPermission: "Gkh1468.Dictionaries.PassportStruct.Edit"));
            map.AddRoute(new ClientRoute("myokipassports/", "B4.controller.providerpassport.MyOki", requiredPermission: "Gkh1468.Passport.MyOki.View"));
            map.AddRoute(new ClientRoute("housepassports/", "B4.controller.passport.House", requiredPermission: "Gkh1468.Passport.House.View"));
            map.AddRoute(new ClientRoute("housepassports_{id}/", "B4.controller.passport.House", "openHousePanel", "Gkh1468.Passport.House.View"));
            map.AddRoute(new ClientRoute("myhousepassports/", "B4.controller.providerpassport.MyHouse", requiredPermission: "Gkh1468.Passport.MyHouse.View"));
            map.AddRoute(new ClientRoute("okipassport/", "B4.controller.passport.Oki", requiredPermission: "Gkh1468.Passport.Oki.View"));
            map.AddRoute(new ClientRoute("okipassport_{id}/", "B4.controller.passport.Oki", "openOkiPanel", requiredPermission: "Gkh1468.Passport.Oki.View"));
            map.AddRoute(new ClientRoute("housepassports_{id}/", "B4.controller.passport.House", "openHousePanel", requiredPermission: "Gkh1468.Passport.House.View"));
            map.AddRoute(new ClientRoute("okipasspeditor/{id}/", "B4.controller.Passport", "okipasspeditor", requiredPermission: "Gkh1468.Passport.Oki.View"));
            map.AddRoute(new ClientRoute("housepasspeditor/{id}/", "B4.controller.Passport", "housepasspeditor", requiredPermission: "Gkh1468.Passport.House.View"));

            map.AddRoute(new ClientRoute("municipality", "B4.controller.dict.Municipality", requiredPermission: "Gkh.Dictionaries.Municipality.View"));
            map.AddRoute(new ClientRoute("publicservice", "B4.controller.dict.PublicService", requiredPermission: "Gkh1468.Dictionaries.PublicService.View"));

            map.AddRoute(new ClientRoute("publicservorg", "B4.controller.PublicServOrg", requiredPermission: "Gkh.RealityObject.Register.PublicServOrg.View"));
            map.AddRoute(new ClientRoute("publicservorgedit/{id}", "B4.controller.publicservorg.Navigation", requiredPermission: "Gkh.Orgs.DeliveryAgent.View"));
            map.AddRoute(new ClientRoute("publicservorgedit/{id}/edit", "B4.controller.publicservorg.Edit", requiredPermission: "Gkh.Orgs.DeliveryAgent.Edit"));
            map.AddRoute(new ClientRoute("publicservorgedit/{id}/contract", "B4.controller.publicservorg.Contract", requiredPermission: "Gkh.Orgs.DeliveryAgent.Municipality.View"));

            map.AddRoute(new ClientRoute("typecustomer", "B4.controller.dict.TypeCustomer", requiredPermission: "Gkh1468.Dictionaries.TypeCustomer.View"));

            map.AddRoute(new ClientRoute("importfrom1468rf/", "B4.controller.ImportFrom1468", requiredPermission: "Gkh1468.Passport.ImportFrom1468.View"));
			map.AddRoute(new ClientRoute("realityobjectedit/{id}/publicservorg", "B4.controller.realityobj.PublicServOrg", requiredPermission: "Gkh.RealityObject.Register.PublicServOrg.Edit"));
        }
    }
}
