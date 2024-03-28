namespace Bars.B4.Modules.Analytics.Reports.Permissions
{
    public class ReportsPermissionMap : PermissionMap
    {
        public ReportsPermissionMap()
        {
            this.Namespace("B4.Analytics.Reports", "Конструктор отчетов");
            this.Permission("B4.Analytics.Reports.ReportPanel.View", "Панель отчетов - Просмотр");
            this.Permission("B4.Analytics.Reports.ReportCustoms.View", "Замена шаблонов - Просмотр");
            this.Permission("B4.Analytics.Reports.StoredReports.View", "Справочник отчетов - Просмотр");
            this.Permission("B4.Analytics.Reports.StoredReports.Connection", "Подключение к базе");
            this.Permission("B4.Analytics.Reports.Kp60Reports.View", "Отчеты КП60");
        }
    }
}