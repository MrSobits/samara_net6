namespace Bars.Esia.OAuth20.App.Enums
{
    /// <summary>
    /// Тип запроса для получения маркера доступа
    /// </summary>
    public enum TokenRequest
    {
        /// <summary>
        /// По авторизационному коду
        /// </summary>
        ByAuthCode = 1,

        /// <summary>
        /// По токену обновления
        /// </summary>
        ByRefresh = 2,

        /// <summary>
        /// На основе полномочий системы
        /// </summary>
        ByCredential = 3
    }
}