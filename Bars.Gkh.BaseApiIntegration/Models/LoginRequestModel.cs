namespace Bars.Gkh.BaseApiIntegration.Models
{
    /// <summary>
    /// Модель для входных параметров для аутентификации
    /// </summary>
    public class LoginRequestModel
    {
        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Пароль пользователя
        /// </summary>
        public string Password { get; set; }
    }
}