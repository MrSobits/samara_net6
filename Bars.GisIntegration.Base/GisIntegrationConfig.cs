namespace Bars.GisIntegration.Base
{
    /// <summary>
    /// Настройки интеграции с ГИС РФ
    /// </summary>
    public class GisIntegrationConfig
    {
        /// <summary>
        /// Подписывать сообщение
        /// </summary>
        public virtual bool SingXml { get; set; }

        /// <summary>
        /// Указывать логин/пароль
        /// </summary>
        public virtual bool UseLoginCredentials { get; set; }

        /// <summary>
        /// Логин
        /// </summary>
        public virtual string Login { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public virtual string Password { get; set; }
    }
}