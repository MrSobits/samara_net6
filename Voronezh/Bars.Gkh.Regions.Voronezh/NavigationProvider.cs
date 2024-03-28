namespace Bars.Gkh.Regions.Voronezh
{
    using Bars.B4;
    using Bars.Gkh.TextValues;

    /// <summary>
    /// Меню, навигация
    /// </summary>
    public class NavigationProvider : INavigationProvider
    {
        public IMenuItemText MenuItemText { get; set; }

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
            root.Add("Администрирование").Add("Импорты").Add("Импорт выписок Ростеестра (старый)", "dateareaowner").AddRequiredPermission("Rosreg.AllRosreg.Import.View");
            root.Add("Администрирование").Add("Импорты").Add("Импорт выписок Ростеестра", "rosregextract").AddRequiredPermission("Rosreg.AllRosreg.Import.View");
            root.Add("Региональный фонд").Add("Счета").Add("Выписки из Росреестра", "rosregextractdesc").AddRequiredPermission("Rosreg.AllRosreg.RoomEGRN.View");

            root.Add("Претензионная работа").Add("Основания претензионной работы").Add("Архив ПИР", "flattenedclaimwork").AddRequiredPermission("Clw.FlattenedClaimWork.View");
            root.Add("Претензионная работа").Add("Основания претензионной работы").Add("Долевые ПИР", "partialclaimwork").AddRequiredPermission("Clw.FlattenedClaimWork.View");
        }
    }
}