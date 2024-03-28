using Bars.B4;
namespace Bars.Gkh.RegOperator.Regions.Tyumen
{
    public class ClientRouteMapRegistrar: IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("requeststateperson", "B4.controller.RequestStatePerson", requiredPermission: "Gkh.Dictionaries.RequestStatePerson.View"));
            map.AddRoute(new ClientRoute("requeststate", "B4.controller.RequestState", requiredPermission: "Gkh.Dictionaries.RequestStatePerson.View"));
        }
    }
}
