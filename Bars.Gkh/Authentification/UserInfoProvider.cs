namespace Bars.Gkh.Authentification
{
    using Bars.B4.Modules.Analytics.Reports;
    using Bars.B4.Modules.Security;

    /// <summary>
    /// Реализация интерфейса получения информации о пользователях
    /// </summary>
    public class UserInfoProvider : IUserInfoProvider
    {
        public IGkhUserManager GkhUserManager { get; set; }

        /// <inheritdoc />
        public User GetActiveUser()
        {
            return this.GkhUserManager.GetActiveUser();
        }
    }
}