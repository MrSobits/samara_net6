namespace Bars.Gkh.Gasu
{
    using Bars.B4;

    public class ClientRouteMapRegistrar : IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("gasuindicator", "B4.controller.GasuIndicator", requiredPermission: "Administration.ExportData.GasuIndicator.View"));

            map.AddRoute(new ClientRoute("gasuindicatorvalue", "B4.controller.GasuIndicatorValue", requiredPermission: "Administration.ExportData.GasuIndicatorValue.View"));
        }
    }
}