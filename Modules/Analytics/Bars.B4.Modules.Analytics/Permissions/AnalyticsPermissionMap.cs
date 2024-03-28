namespace Bars.B4.Modules.Analytics.Permissions
{
    public class AnalyticsPermissionMap: PermissionMap
    {
        public AnalyticsPermissionMap()
        {
            Namespace("B4.Analytics", "Модуль аналитики");
            Permission("B4.Analytics.DataSources.View", "Источники данных - Просмотр");
        }
    }
}
