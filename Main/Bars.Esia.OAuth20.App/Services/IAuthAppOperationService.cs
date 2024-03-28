namespace Bars.Esia.OAuth20.App.Services
{
    using System.Collections.Generic;

    using Bars.B4.Utils;
    using Bars.Esia.OAuth20.App.Entities;

    /// <summary>
    /// Сервис операций приложения
    /// </summary>
    public interface IAuthAppOperationService
    {
        /// <summary>
        /// Сформировать и вернуть Uri-перенаправление для получения кода доступа
        /// </summary>
        string GetRedirectUri(DynamicDictionary operationParams);

        /// <summary>
        /// Получить маркер доступа
        /// </summary>
        EsiaOAuthToken GetOAuthToken(DynamicDictionary operationParams);

        /// <summary>
        /// Получить сведения о пользователе
        /// </summary>
        EsiaPersonInfo GetPersonInfo(DynamicDictionary operationParams);

        /// <summary>
        /// Получить основные сведения об организациях пользователя
        /// </summary>
        /// <remarks>
        /// Предоставляет основной набор данных об организации.
        /// Для получения дополнительной информации необходимо воспользоваться GetOrganizationInfo
        /// </remarks>
        IEnumerable<EsiaPersonOrganizationInfo> GetPersonOrganizations(DynamicDictionary operationParams);

        /// <summary>
        /// Получить сведения о контактах пользователя
        /// </summary>
        IEnumerable<EsiaContactInfo> GetPersonContacts(DynamicDictionary operationParams);

        /// <summary>
        /// Получить сведения об адресах пользователя
        /// </summary>
        IEnumerable<EsiaAddressInfo> GetPersonAddresses(DynamicDictionary operationParams);

        /// <summary>
        /// Получить сведения об организации
        /// </summary>
        /// <remarks>
        /// Предоставляет расширенный набор данных об организации.
        /// Маркер доступа должен быть получен с соответствующим scope
        /// </remarks>
        EsiaOrganizationInfo GetOrganizationInfo(DynamicDictionary operationParams);
    }
}