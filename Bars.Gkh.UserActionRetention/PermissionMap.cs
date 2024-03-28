namespace Bars.Gkh.UserActionRetention
{
    public class PermissionMap : B4.PermissionMap
    {
        public PermissionMap()
        {
            #region Администрирование

            Namespace("Administration.UserActionRetention", "Журнал действий пользователя");
            Permission("Administration.UserActionRetention.View", "Просмотр раздела");

            #endregion
        }
    }
}
