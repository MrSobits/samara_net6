namespace Bars.B4.Modules.Analytics.Reports.Web
{
    using Bars.B4;

    /// <summary>
    /// 
    /// </summary>
    public class ReportsNavigationProvider : INavigationProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="root"></param>
        public void Init(MenuItem root)
        {
            root.Add("Отчеты").Add("Отчеты");
            root.Add("Отчеты").Add("Отчеты").Add("Замена шаблонов", "reportcustoms").AddRequiredPermission("B4.Analytics.Reports.ReportCustoms.View");
            root.Add("Отчеты").Add("Отчеты").Add("Справочник отчетов", "storedreports").AddRequiredPermission("B4.Analytics.Reports.StoredReports.View");
            root.Add("Отчеты").Add("Отчеты").Add("Панель отчетов", "reportpanel").AddRequiredPermission("B4.Analytics.Reports.ReportPanel.View");
        }

        /// <summary>
        /// 
        /// </summary>
        public string Key
        {
            get { return MainNavigationInfo.MenuName; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Description
        {
            get { return MainNavigationInfo.MenuDescription; }
        }
    }
}
