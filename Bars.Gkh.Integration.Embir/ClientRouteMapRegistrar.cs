using Bars.B4;

namespace Bars.Gkh.Integration.Embir
{
    public class ClientRouteMapRegistrar : IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("importembir", "B4.controller.import.Embir", requiredPermission: "Import.Embir.View"));
        }
    }
}