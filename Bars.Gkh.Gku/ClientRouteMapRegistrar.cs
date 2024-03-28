namespace Bars.Gkh.Gku
{
    using Bars.B4;

    public class ClientRouteMapRegistrar: IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("gkuinfo", "B4.controller.GkuInfo", requiredPermission: "Gkh.GkuInfo.View"));
            map.AddRoute(new ClientRoute("gkutarif", "B4.controller.dict.GkuTarifGji", requiredPermission: "GkhGji.Dict.GkuTariff.View"));
        }
    }
}
