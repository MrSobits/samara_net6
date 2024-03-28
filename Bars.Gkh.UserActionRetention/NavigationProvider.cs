namespace Bars.Gkh.UserActionRetention
{
    using B4;

    public class NavigationProvider : INavigationProvider
    {
        public void Init(MenuItem root)
        {
            root.Add("Администрирование").Add("Аудит").Add("Журнал действий пользователя", "useractionretention")
                .AddRequiredPermission("Administration.UserActionRetention.View").WithIcon("reminderHead");
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