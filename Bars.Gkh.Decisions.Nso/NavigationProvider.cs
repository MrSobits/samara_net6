namespace Bars.Gkh.Decisions.Nso
{
    using B4;

    public class NavigationProvider: INavigationProvider
    {
        public void Init(MenuItem root)
        {
            root
                .Add("Жилищная инспекция")
                .Add("Реестр уведомлений")
                .Add("Сводный реестр уведомлений о решениях общего собрания", "decisionnotification")
                .AddRequiredPermission("Ovrhl.RegistryNotifications.View");
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