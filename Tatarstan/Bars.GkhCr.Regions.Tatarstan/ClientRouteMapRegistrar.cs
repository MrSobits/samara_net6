namespace Bars.GkhCr.Regions.Tatarstan
{
    using Bars.B4;

    public class ClientRouteMapRegistrar : IClientRouteMapRegistrar
    {
        /// <inheritdoc />
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("outdoorprogram", "B4.controller.dict.RealityObjectOutdoorProgram", requiredPermission: "GkhCr.OutdoorProgram.View"));
            map.AddRoute(new ClientRoute("workrealityobjectoutdoor", "B4.controller.dict.WorkRealityObjectOutdoor", requiredPermission: "GkhCr.Dict.WorkRealityObjectOutdoor.View"));
            map.AddRoute(new ClientRoute("objectoutdoorcr", "B4.controller.objectoutdoorcr.ObjectOutdoorCr", requiredPermission: "GkhCr.ObjectOutdoorCr.View"));
            map.AddRoute(new ClientRoute("objectoutdoorcredit/{id}", "B4.controller.objectoutdoorcr.Navi", requiredPermission: "GkhCr.ObjectOutdoorCr.View"));
            map.AddRoute(new ClientRoute("objectoutdoorcredit/{id}/edit", "B4.controller.objectoutdoorcr.Edit", requiredPermission: "GkhCr.ObjectOutdoorCr.View"));
            map.AddRoute(new ClientRoute("objectoutdoorcredit/{id}/typeworkrealityobjectoutdoor", "B4.controller.objectoutdoorcr.TypeWorkRealityObjectOutdoor", requiredPermission: "GkhCr.ObjectOutdoorCr.View"));
            map.AddRoute(new ClientRoute("elementoutdoor", "B4.controller.dict.ElementOutdoor", requiredPermission: "GkhCr.Dict.ElementOutdoor.View"));
        }
    }
}
