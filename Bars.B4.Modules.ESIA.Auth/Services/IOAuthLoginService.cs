namespace Bars.B4.Modules.ESIA.Auth.Services
{
    using System.Web;

    using Bars.B4.Modules.ESIA.Auth.Dto;

    /// <summary>
    /// Интерфейс сервиса OAuth авторизации
    /// </summary>
    public interface IOAuthLoginService
    {
        /// <summary>
        /// Получить информацию о пользователе
        /// </summary>
        IDataResult<UserInfoDto> GetUserInfo(string code);

        /// <summary>
        /// Сопоставить организацию с контрагентом
        /// </summary>
        IDataResult MatchOrganizationWithContragent(UserInfoDto userInfo);

        /// <summary>
        /// Выполнить действия логина
        /// </summary>
        IDataResult PerformLoginActions(UserInfoDto userInfo);

        /// <summary>
        /// Привязать аккаунт ЕСИА
        /// </summary>
        IDataResult LinkEsiaAccount(BaseParams baseParams, HttpRequestBase request);
    }
}