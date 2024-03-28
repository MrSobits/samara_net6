namespace Bars.GkhGji.Regions.Nso
{
    using B4;

    public class ClientRouteMapRegistrar : IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("reminderappealcits", "B4.controller.ReminderAppealCits", requiredPermission: "GkhGji.ManagementTask.ReminderAppealCits.View"));
            map.AddRoute(new ClientRoute("mkdchangenotification", "B4.controller.MkdChangeNotification", requiredPermission: "GkhGji.MkdChangeNotification.View"));
            map.AddRoute(new ClientRoute("protocol197", "B4.controller.protocol197.Protocol197", requiredPermission: "GkhGji.DocumentsGji.Protocol197.View"));
        }
    }
}