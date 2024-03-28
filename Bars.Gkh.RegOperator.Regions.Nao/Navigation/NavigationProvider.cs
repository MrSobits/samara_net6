using Bars.B4;

namespace Bars.Gkh.RegOperator.Regions.Nao.Navigation
{
    public class NavigationProvider : INavigationProvider
    {
        public string Key =>  MainNavigationInfo.MenuName;

        public string Description => MainNavigationInfo.MenuDescription;

        public void Init(MenuItem root)
        {
            var admImportNode = root.Add("Администрирование").Add("Импорты");
            admImportNode.Add("Импорт помещений", "quarterimport").AddRequiredPermission("Import.QuarterImport");
        }
    }
}
