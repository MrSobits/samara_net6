namespace Bars.Gkh.Gis.ConfigSections
{
    using System.ComponentModel;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;

    public class EgsoIntegrationConfig : IGkhConfigSection
    {
        /// <summary>
        /// Максимальная длина поля
        /// </summary>
        public const int MaxWidth = 720;

        /// <summary>
        /// Адрес сервиса
        /// </summary>
        [GkhConfigProperty(DisplayName = "Адрес сервиса")]
        [DefaultValue("https://ias.tatar.ru/service/")]
        public virtual string ServiceAddress { get; set; }

        /// <summary>
        /// Логин
        /// </summary>
        [GkhConfigProperty(DisplayName = "Логин")]
        [DefaultValue("133375")]
        public virtual string Login { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        [GkhConfigProperty(DisplayName = "Пароль")]
        [DefaultValue("12345")]
        public virtual string Password { get; set; }
    }
}