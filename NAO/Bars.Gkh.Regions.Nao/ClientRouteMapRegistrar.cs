using Bars.B4;
namespace Bars.Gkh.Regions.Nao
{
    public class ClientRouteMapRegistrar : IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("assberbankclient", "B4.controller.ASSberbankClient", requiredPermission: "Gkh.Dictionaries.ASSberbankClient"));
        }
    }
}
