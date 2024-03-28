namespace Bars.Gkh.Gis.ConfigSections
{
    using System.ComponentModel;
    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;

    /// <summary>
    /// Настройки интеграции с Народным Контролем
    /// </summary>
    public class PublicControlIntegrationConfig : IGkhConfigSection
    {
        /// <summary>
        /// Адрес сервиса
        /// </summary>
        [GkhConfigProperty(DisplayName = "Адрес сервиса")]
        [DefaultValue("https://api.tatar.ru/open-gov")]
        public virtual string ServiceAddress { get; set; }

        /// <summary>
        /// AppToken
        /// </summary>
        [GkhConfigProperty(DisplayName = "AppToken")]
        [DefaultValue("ed8f5b7e74398143b43a93dc753618ae")]
        public virtual string AppToken { get; set; }
    }
}