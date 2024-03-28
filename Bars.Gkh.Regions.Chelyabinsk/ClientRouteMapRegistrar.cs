namespace Bars.Gkh.Regions.Chelyabinsk
{
    using Bars.B4;

    public class ClientRouteMapRegistrar : IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("rosregextract", "B4.controller.RosRegExtract"));
            map.AddRoute(new ClientRoute("rosregextractbig", "B4.controller.RosRegExtractBig"));
        }
    }
}