namespace Bars.Gkh.Regions.Tyumen
{
    using Bars.B4;

    public class ClientRouteMapRegistrar: IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("lifttype", "B4.controller.dict.TypeLift", requiredPermission: "Gkh.Dictionaries.TypeLift.View"));
            map.AddRoute(new ClientRoute("liftmodel", "B4.controller.dict.ModelLift", requiredPermission: "Gkh.Dictionaries.ModelLift.View"));
            map.AddRoute(new ClientRoute("liftcabin", "B4.controller.dict.CabinLift", requiredPermission: "Gkh.Dictionaries.CabinLift.View"));
            map.AddRoute(new ClientRoute("typeliftshaft", "B4.controller.dict.TypeLiftShaft", requiredPermission: "Gkh.Dictionaries.TypeLiftShaft.View"));
            map.AddRoute(new ClientRoute("typeliftdrivedoors", "B4.controller.dict.TypeLiftDriveDoors", requiredPermission: "Gkh.Dictionaries.TypeLiftDriveDoors.View"));
            map.AddRoute(new ClientRoute("typeliftmashineroom", "B4.controller.dict.TypeLiftMashineRoom", requiredPermission: "Gkh.Dictionaries.TypeLiftMashineRoom.View"));
            map.AddRoute(new ClientRoute("realityobjectexaminationimport", "B4.controller.import.RealityObjectExaminationImport", requiredPermission: "Import.RealityObjectExaminationImport.View"));
            map.AddRoute(new ClientRoute("realityobjectedit/{id}/lift", "B4.controller.realityobj.Lift", requiredPermission: "Gkh.RealityObject.Register.Lift"));

            //Ребинд роутов на ГИС
            map.RegisterRoute("gisintegration", "B4.controller.integrations.GisIntegration", requiredPermission: "Administration.OutsideSystemIntegrations.Gis.View");
            map.RegisterRoute("gisintegrationsettings", "B4.controller.integrations.GisIntegrationSettings", requiredPermission: "Administration.OutsideSystemIntegrations.Gis");
            map.RegisterRoute("delegacy", "B4.controller.delegacy.Delegacy", requiredPermission: "Administration.OutsideSystemIntegrations.Delegacy.View");
            map.RegisterRoute("gisrole", "B4.controller.gisrole.GisRole", requiredPermission: "Administration.OutsideSystemIntegrations.GisRole.View");
            map.RegisterRoute("riscontragentrole", "B4.controller.gisrole.RisContragentRole", requiredPermission: "Administration.OutsideSystemIntegrations.RisContragentRole.View");
        }
    }
}