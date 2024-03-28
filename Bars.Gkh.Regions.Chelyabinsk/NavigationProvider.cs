namespace Bars.Gkh.Regions.Chelyabinsk
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
            root.Add("Администрирование").Add("Импорты").Add("Импорт выписок Ростеестра", "rosregextract").AddRequiredPermission("Rosreg.AllRosreg.RoomEGRN.View"); ;
            root.Add("Региональный фонд").Add("Счета").Add("Выписки из Росреестра", "rosregextractbig").AddRequiredPermission("Rosreg.AllRosreg.RoomEGRN.View"); ;

        }
    }
}