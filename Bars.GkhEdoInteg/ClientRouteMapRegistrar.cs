namespace Bars.GkhEdoInteg
{
    using Bars.B4;

    public class ClientRouteMapRegistrar : IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("inspectoredointeg", "B4.controller.dict.InspectorEdoInteg", requiredPermission: "GkhEdoInteg.Compare.Inspector.View"));
            map.AddRoute(new ClientRoute("kindstatementedointeg", "B4.controller.dict.KindStatementEdoInteg", requiredPermission: "GkhEdoInteg.Compare.KindStatement.View"));
            map.AddRoute(new ClientRoute("revenueformedointeg", "B4.controller.dict.RevenueFormEdoInteg", requiredPermission: "GkhEdoInteg.Compare.RevenueForm.View"));
            map.AddRoute(new ClientRoute("revenuesourceedointeg", "B4.controller.dict.RevenueSourceEdoInteg", requiredPermission: "GkhEdoInteg.Compare.RevenueSource.View"));

            map.AddRoute(new ClientRoute("edolog", "B4.controller.LogEdo", requiredPermission: "GkhEdoInteg.LogEdo.View"));
            map.AddRoute(new ClientRoute("edologrequests", "B4.controller.LogEdoRequests", requiredPermission: "GkhEdoInteg.LogEdoRequests.View"));
            map.AddRoute(new ClientRoute("sendemsed", "B4.controller.SendEmsed", requiredPermission: "GkhEdoInteg.SendEdo.View"));

        }
    }
}