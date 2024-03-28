namespace Bars.Gkh.Gasu
{
    using Bars.B4;

    public class NavigationProvider : INavigationProvider
    {
        public void Init(MenuItem root)
        {
            root.Add("Администрирование")
                .Add("Экспорт данных")
                .Add("Данные для 'ГАС Управление'", "managementsysimport")
                .AddRequiredPermission("Administration.ExportManagSys.View");

            root.Add("Администрирование")
                .Add("Экспорт данных")
                .Add("Данные для 'ГАС Управление' (КР)", "overhaultogasu")
                .AddRequiredPermission("Administration.ExportOverhaulToGasu.View");

            root.Add("Администрирование")
                .Add("Экспорт данных")
                .Add("Показатели ГАСУ", "gasuindicator")
                .AddRequiredPermission("Administration.ExportData.GasuIndicator.View");

            root.Add("Администрирование")
                .Add("Экспорт данных")
                .Add("Сведения для отправки в ГАСУ", "gasuindicatorvalue")
                .AddRequiredPermission("Administration.ExportData.GasuIndicatorValue.View");
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