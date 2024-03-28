namespace Bars.Gkh.Reforma.Navigation
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
            root.Add("Раскрытие информации").Add("Раскрытие информации по 731 (988) ПП РФ").Add("Настройки интеграции с Реформой ЖКХ", "reforma_params").AddRequiredPermission("GkhDi.Reforma.ChangeParams");
            root.Add("Раскрытие информации").Add("Раскрытие информации по 731 (988) ПП РФ").Add("Восстановление данных Реформы ЖКХ", "reforma_restore").AddRequiredPermission("GkhDi.Reforma.Restore");
            root.Add("Администрирование").Add("Логи").Add("Лог интеграции с Реформой ЖКХ", "reforma_synclog").AddRequiredPermission("GkhDi.Reforma.SyncLog");
            root.Add("Справочники").Add("Реформа ЖКХ").Add("Отчетные периоды", "reporting_period_dict").AddRequiredPermission("GkhDi.Reforma.Dictionaries.ReportingPeriod.View");
        }
    }
}