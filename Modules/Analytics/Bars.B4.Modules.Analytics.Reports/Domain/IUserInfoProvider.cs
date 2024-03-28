namespace Bars.B4.Modules.Analytics.Reports
{
    using Bars.B4.Modules.Security;

    /// <summary>
    /// Интерфейс-обертка для UserManager (для получения информации о пользователях)
    /// </summary>
    public interface IUserInfoProvider
    {
        /// <summary>
        /// Получить активного пользователя
        /// </summary>
        /// <returns>Активный пользователь</returns>
        User GetActiveUser();
    }
}
