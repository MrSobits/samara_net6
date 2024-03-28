namespace Bars.GkhCalendar
{
    using Bars.B4;

    public class ClientRouteMapRegistrar : IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("industrialcalendar", "B4.controller.IndustrialCalendar", requiredPermission: "Administration.Calendar.View"));
            map.AddRoute(new ClientRoute("AppointmentGrid", "B4.controller.Appointment", requiredPermission: "Administration.Calendar.View"));
        }
    }
}
