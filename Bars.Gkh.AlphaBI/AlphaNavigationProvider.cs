namespace Bars.Gkh.AlphaBI
{
    using B4;

    public class AlphaNavigationProvider : INavigationProvider
    {
        public void Init(MenuItem root)
        {
            root.Add("Отчеты");
            root.Add("Отчеты").Add("Отчеты");
            root.Add("Отчеты").Add("Отчеты").Add("OLAP аналитика", "olap").AddRequiredPermission("OLAP.Alpha");
        }

        public string Key
        {
            get { return MainNavigationInfo.MenuName; }
        }

        public string Description
        {
            get { return MainNavigationInfo.MenuDescription; }
        }
    }
}