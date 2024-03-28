namespace Bars.Gkh.Gis.ConfigSections
{
    using System.ComponentModel;
    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;

    /// <summary>
    /// Настройки интеграции с Открытым Татарстаном
    /// </summary>
    public class OpenTatarstanIntegrationConfig : IGkhConfigSection
    {
        /// <summary>
        /// Адрес сервиса
        /// </summary>
        [GkhConfigProperty(DisplayName = "Адрес сервиса")]
        [DefaultValue("http://demo-ias.e-kazan.ru/service/")]
        public virtual string ServiceAddress { get; set; }

        /// <summary>
        /// Логин
        /// </summary>
        [GkhConfigProperty(DisplayName = "Логин")]
        [DefaultValue("117642")]
        public virtual string Login { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        [GkhConfigProperty(DisplayName = "Пароль")]
        [DefaultValue("OpaTaz09")]
        public virtual string Password { get; set; }
    }
}