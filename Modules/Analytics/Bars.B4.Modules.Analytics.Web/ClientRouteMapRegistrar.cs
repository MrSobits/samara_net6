namespace Bars.B4.Modules.Analytics.Web
{
    using Bars.B4;

    public class ClientRouteMapRegistrar : IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("storedfilters", "B4.controller.al.StoredFilter"));
            map.AddRoute(new ClientRoute("datasources", "B4.controller.al.DataSource"));
        }
    }
}
