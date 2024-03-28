namespace Bars.GkhDi.Regions.Tatarstan
{
    using Bars.B4;

    public class ClientRouteMapRegistrar: IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("measuresreducecosts", "B4.controller.dict.MeasuresReduceCosts", requiredPermission: "GkhDi.Dict.MeasuresReduceCosts.View"));
        }
    }
}
