namespace Bars.B4.Modules.ESIA.Auth.Services
{
    using System.Collections.Generic;

    using Bars.Esia.OAuth20.App.Entities;

    /// <summary>
    /// Интерфейс сервиса интеграции с приложением авторизации
    /// </summary>
    public interface IAuthAppIntegrationService
    {
        /// <summary>
        /// Получить Uri для получения кода доступа
        /// </summary>
        string GetRedirectUrl(string callbackUri = null);

        /// <summary>
        /// Получить маркер доступа по коду доступа
        /// </summary>
        EsiaOAuthToken GetOAuthToken(string code);

        /// <summary>
        /// Получить сведения о пользователе
        /// </summary>
        EsiaPersonInfo GetPersonInfo(EsiaOAuthToken esiaOAuthToken);

        /// <summary>
        /// Получить сведения об организациях пользователя
        /// </summary>
        IList<EsiaPersonOrganizationInfo> GetPersonOrganizationsInfo(EsiaOAuthToken esiaOAuthToken);
    }
}