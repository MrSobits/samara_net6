namespace Bars.Gkh.Overhaul.Regions.Saha
{
    using Bars.B4;

    public class NavigationProvider : INavigationProvider
    {
        public void Init(MenuItem root)
        {
            root.Add("Отчеты").Add("Отчеты").Add("Отчеты КП60", "kp60reports").AddRequiredPermission("B4.Analytics.Reports.Kp60Reports.View");
        }

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
    }
}