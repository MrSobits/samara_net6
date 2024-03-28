namespace Sobits.GisGkh
{
    using Bars.B4;

    public class ClientRouteMapRegistrar : IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            //Ребинд роутов на ГИС
            map.RegisterRoute("gisintegration", "B4.controller.integrations.GisIntegration", requiredPermission: "Administration.OutsideSystemIntegrations.Gis.View");
            map.RegisterRoute("gisintegrationsettings", "B4.controller.integrations.GisIntegrationSettings", requiredPermission: "Administration.OutsideSystemIntegrations.Gis");
            map.RegisterRoute("delegacy", "B4.controller.delegacy.Delegacy", requiredPermission: "Administration.OutsideSystemIntegrations.Delegacy.View");
            map.RegisterRoute("gisrole", "B4.controller.gisrole.GisRole", requiredPermission: "Administration.OutsideSystemIntegrations.GisRole.View");
            map.RegisterRoute("riscontragentrole", "B4.controller.gisrole.RisContragentRole", requiredPermission: "Administration.OutsideSystemIntegrations.RisContragentRole.View");
            map.AddRoute(new ClientRoute("gisgkhintegration", "B4.controller.GisGkhIntegration", requiredPermission: "Administration.OutsideSystemIntegrations.GisGkh.View"));
            map.AddRoute(new ClientRoute("gisgkhintegrationinsmev", "B4.controller.GisGkhIntegration", requiredPermission: "GkhGji.GisGkh.View"));
        }
    }
}