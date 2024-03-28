namespace Bars.Gkh.AlphaBI
{
    using B4;

    public class AlphaClientRouteMapRegistrar : IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("olap", "B4.controller.AlphaBi", requiredPermission: "OLAP.Alpha"));
        }
    }
}