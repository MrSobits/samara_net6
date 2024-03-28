namespace Bars.Gkh.Decisions.Nso
{
    using B4;

    public class ClientRouteMapRegistrar : IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("decisionnotification", "B4.controller.DecisionNotification", requiredPermission: "Ovrhl.RegistryNotifications.View"));
            map.AddRoute(new ClientRoute("existingsolutions", "B4.controller.realityobj.ExistingSolutions"));

            map.AddRoute(new ClientRoute("realityobjectedit/{id}/decisionprotocol", "B4.controller.realityobj.DecisionProtocol", requiredPermission: "Gkh.RealityObject.Register.DecisionProtocolsViewCreate.View"));
            map.AddRoute(new ClientRoute("realityobjectedit/{id}/govdecisionprotocol", "B4.controller.realityobj.GovDecisionProtocol", requiredPermission: "Gkh.RealityObject.Register.GovProtocolDecisionViewCreate.View"));
            map.AddRoute(new ClientRoute("realityobjectedit/{id}/decisionhistory", "B4.controller.realityobj.DecisionHistory", requiredPermission: "Gkh.RealityObject.Register.DecisionHistory.View"));
        }
    }
}