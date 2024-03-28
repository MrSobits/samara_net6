namespace Sobits.RosReg
{
    using Bars.B4;

    public class ClientRouteMapRegistrar : IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("extractimport", "B4.controller.ExtractImport"));
            map.AddRoute(new ClientRoute("extract", "B4.controller.Extract"));
            map.AddRoute(new ClientRoute("extractegrn", "B4.controller.ExtractEgrn"));
        }
    }
}