using Bars.B4;

namespace Bars.Gkh.Integration.Embir
{
    public class NavigationProvider: INavigationProvider
    {
        public void Init(MenuItem root)
        {
            root
                .Add("Администрирование")
                .Add("Импорты")
                .Add("Импорт из ЕМБИР", "importembir").AddRequiredPermission("Import.Embir.View");
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