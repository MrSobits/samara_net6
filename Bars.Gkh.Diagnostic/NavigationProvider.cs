namespace Bars.Gkh.Diagnostic
{
    using B4;

    /// <summary>
    ///     Меню, навигация
    /// </summary>
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
            root.Add("Администрирование").Add("Диагностика").Add("Диагностика", "diagnostic").AddRequiredPermission("Administration.Diagnostic.View");
        }
    }
}