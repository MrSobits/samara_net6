namespace Bars.Gkh.Reforma
{
    using Bars.B4;

    public class ClientRouteMapRegistrar : IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("reforma_params", "B4.controller.SyncParams", requiredPermission: "GkhDi.Reforma.ChangeParams"));
            map.AddRoute(new ClientRoute("reforma_synclog", "B4.controller.SyncLog", requiredPermission: "GkhDi.Reforma.SyncLog"));
            map.AddRoute(new ClientRoute("reporting_period_dict", "B4.controller.dict.ReportingPeriod", requiredPermission: "GkhDi.Reforma.Dictionaries.ReportingPeriod.View"));

            map.AddRoute(new ClientRoute("reforma_restore", "B4.controller.ReformaRestore", requiredPermission: "GkhDi.Reforma.Restore"));

            map.AddRoute(new ClientRoute("realityobjectedit/{id}/reforma", "B4.controller.robject.ReformaRobject", requiredPermission: "Gkh.RealityObject.Register.Reforma.View"));
        }
    }
}