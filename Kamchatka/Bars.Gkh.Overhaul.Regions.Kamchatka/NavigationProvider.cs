namespace Bars.Gkh.Overhaul.Regions.Kamchatka
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
            root.Add("Администрирование").Add("Импорт жилых домов").Add("Импорт жилых домов для Камчатки", "kamchrealityobjimport").AddRequiredPermission("Import.KamchatkaRealtyObjectImport.View");

            var menu = root.Add("Аналитика.Управление").WithIcon("an_mon");

            menu.Add("Аналитика. Управление ЦП. Мониторинг").Add("Мониторинг", "monitoring").WithIcon("mon");
            menu.Add("Аналитика. Управление ЦП. Мониторинг").Add("Аналитика", "analytics").WithIcon("an");
            menu.Add("Аналитика. Управление ЦП. Мониторинг").Add("Управление ЦП", "ucp").WithIcon("ucp");
        }
    }
}