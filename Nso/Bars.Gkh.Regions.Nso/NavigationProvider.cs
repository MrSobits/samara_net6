namespace Bars.Gkh.Regions.Nso
{
    using Bars.B4;

    public class NavigationProvider : INavigationProvider
    {
        public void Init(MenuItem root)
        {
            root.Add("Администрирование").Add("Импорты").Add("Импорт УО и привязка их к домам", "manorgrobjectimport").AddRequiredPermission("Import.ManOrgRobjectImport");
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