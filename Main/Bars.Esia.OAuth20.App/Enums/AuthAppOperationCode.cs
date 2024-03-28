namespace Bars.Esia.OAuth20.App.Enums
{
    /// <summary>
    /// Код операции сервиса авторизации
    /// </summary>
    public enum AuthAppOperationCode
    {
        /// <summary>
        /// Получить URI-перенаправление для получения авторизационного кода
        /// </summary>
        GetRedirectUri = 1,

        /// <summary>
        /// Получить маркер доступа
        /// </summary>
        GetOAuthToken = 2,

        /// <summary>
        /// Получить сведения о пользователе
        /// </summary>
        GetPersonInfo = 3,

        /// <summary>
        /// Получить сведения об организациях пользователя
        /// </summary>
        GetPersonOrganizations = 4,

        /// <summary>
        /// Получить сведения о контактах пользователя
        /// </summary>
        GetPersonContacts = 5,

        /// <summary>
        /// Получить сведения об адресах пользователя
        /// </summary>
        GetPersonAddresses = 6,

        /// <summary>
        /// Получить сведения об организации
        /// </summary>
        GetOrganizationInfo = 7
    }
}