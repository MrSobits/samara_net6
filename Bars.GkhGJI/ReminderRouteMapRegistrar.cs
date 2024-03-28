namespace Bars.GkhGji
{
    using Bars.B4;

    public class ReminderRouteMapRegistrar : IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("reminderInspector", "B4.controller.Reminder", "reminderInspector"));
            map.AddRoute(new ClientRoute("reminderHead", "B4.controller.Reminder", "reminderHead"));
            map.AddRoute(new ClientRoute("reminderTaskControl/{colorType}/{inspectorId}", "B4.controller.Reminder", "reminderTaskControl"));
            map.AddRoute(new ClientRoute("reminderTaskState/{colorType}/{typeReminder}", "B4.controller.Reminder", "reminderTaskState"));
        }
    }
}
