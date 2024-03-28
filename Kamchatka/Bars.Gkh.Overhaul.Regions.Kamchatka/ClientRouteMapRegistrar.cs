namespace Bars.Gkh.Overhaul.Regions.Kamchatka
{
    using Bars.B4;

    public class ClientRouteMapRegistrar : IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("kamchrealityobjimport", "B4.controller.import.KamchatkaRealtyObject", requiredPermission: "Import.KamchatkaRealtyObjectImport.View"));
            map.AddRoute(new ClientRoute("monitoring", "B4.controller.OverhaulExtternalLinks", "monitoring"));
            map.AddRoute(new ClientRoute("analytics", "B4.controller.OverhaulExtternalLinks", "analytics"));
            map.AddRoute(new ClientRoute("ucp", "B4.controller.OverhaulExtternalLinks", "ucp"));
        }
    }
}