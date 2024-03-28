namespace Bars.GkhCalendar
{
    using Bars.B4;

    public class GkhCalendarPermissionMap : PermissionMap
    {
        public GkhCalendarPermissionMap()
        {
            Namespace("Administration.Calendar", "Производственный календарь");
            Permission("Administration.Calendar.View", "Просмотр");
        }
    }
}