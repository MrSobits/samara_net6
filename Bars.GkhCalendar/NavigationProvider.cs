namespace Bars.GkhCalendar
{
    using Bars.B4;

    public class NavigationProvider : INavigationProvider
    {
        public string Key
        {
            get
            {
                return MainNavigationInfo.MenuName;
            }
        }

        public string Description
        {
            get
            {
                return MainNavigationInfo.MenuDescription;
            }
        }

        public void Init(MenuItem root)
        {
            root.Add("Администрирование").Add("Производственный календарь").Add("Производственный календарь", "industrialcalendar").AddRequiredPermission("Administration.Calendar.View");
            root.Add("Администрирование").Add("Производственный календарь").Add("Запись на приём", "AppointmentGrid").AddRequiredPermission("Administration.Calendar.View");
        }
    }
}