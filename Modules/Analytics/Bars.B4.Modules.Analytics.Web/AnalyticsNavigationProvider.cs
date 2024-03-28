namespace Bars.B4.Modules.Analytics.Web
{
    using Bars.B4;

    /// <summary>
    /// 
    /// </summary>
    public class AnalyticsNavigationProvider: INavigationProvider
    {
        public void Init(MenuItem root)
        {
            root.Add("Отчеты");
            root.Add("Отчеты").Add("Отчеты");
            // root.Add("Аналитика").Add("Справочники").Add("Фильтры", "storedfilters");
            root.Add("Отчеты").Add("Отчеты").Add("Источники данных", "datasources").AddRequiredPermission("B4.Analytics.DataSources.View");
        }

        public string Key {
            get { return MainNavigationInfo.MenuName; }
        }
        public string Description
        {
            get { return MainNavigationInfo.MenuDescription; }
        }
    }
}
