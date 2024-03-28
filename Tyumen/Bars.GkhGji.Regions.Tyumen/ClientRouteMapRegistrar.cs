namespace Bars.GkhGji.Regions.Tyumen
{
    using Bars.B4;

    public class ClientRouteMapRegistrar : IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("applicantnotification", "B4.controller.suggestion.ApplicantNotification", requiredPermission: "Gkh.Dictionaries.Suggestion.ApplicantNotification.View"));
            map.AddRoute(new ClientRoute("networkoperator", "B4.controller.networkoperator.NetworkOperator", requiredPermission: "Gkh.Orgs.NetworkOperator.View"));
            map.AddRoute(new ClientRoute("techdecision", "B4.controller.dict.TechDecision", requiredPermission: "Gkh.Dictionaries.Techdecision.View"));
            map.AddRoute(new ClientRoute("realityobjectedit/{id}/networkoperator", "B4.controller.networkoperator.NetworkOperatorRealityObject", requiredPermission: "Gkh.Orgs.NetworkOperator.View"));
            map.AddRoute(new ClientRoute("manorglicenseregistergis", "B4.controller.manorglicense.LicenseGis"));
            map.AddRoute(new ClientRoute("smevegrn", "B4.controller.SMEVEGRN", requiredPermission: "GkhGji.SMEV.SMEVEGRN.View"));
        }
    }
}