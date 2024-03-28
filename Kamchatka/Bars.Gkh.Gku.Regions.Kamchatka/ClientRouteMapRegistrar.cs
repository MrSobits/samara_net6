namespace Bars.Gkh.Gku.Regions.Kamchatka
{
    using Bars.B4;

    public class ClientRouteMapRegistrar : IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("billing", "B4.controller.GkuExternalLinks", "billing", requiredPermission: "Ovrhl.Billing"));
        }
    }
}