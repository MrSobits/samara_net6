using Bars.B4;
namespace Bars.Gkh.UserActionRetention
{
    public class ClientRouteMapRegistrar: IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("useractionretention", "B4.controller.UserActionRetention", requiredPermission: "Administration.UserActionRetention.View"));
        }
    }
}
