namespace Bars.Esia.OAuth20.App.Entities
{
    /// <summary>
    /// Токен из ЕСИА
    /// </summary>
    public class EsiaOAuthToken
    {
        /// <summary>
        /// Маркер доступа
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Токен обновления
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// Срок действия маркера (в секундах)
        /// </summary>
        public string ExpiresIn { get; set; }
    }
}