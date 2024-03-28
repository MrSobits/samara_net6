namespace Bars.Gkh.Gis.ConfigSections
{
    using System.ComponentModel;
    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;

    /// <summary>
    /// Настройки интеграции с Открытой Казанью
    /// </summary>
    public class OpenKazanIntegrationConfig : IGkhConfigSection
    {
        /// <summary>
        /// Адрес сервиса
        /// </summary>
        [GkhConfigProperty(DisplayName = "Адрес сервиса")]
        [DefaultValue("https://rest.open.kzn.ru")]
        public virtual string ServiceAddress { get; set; }

        /// <summary>
        /// RestToken
        /// </summary>
        [GkhConfigProperty(DisplayName = "RestToken")]
        [DefaultValue("gisgkh")]
        public virtual string RestToken { get; set; }

        /// <summary>
        /// RestSecret
        /// </summary>
        [GkhConfigProperty(DisplayName = "RestSecret")]
        [DefaultValue("abdf9ec50e7f514137f3d38f2ca2abb3f121d0c8")]
        public virtual string RestSecret { get; set; }

        /// <summary>
        /// Логин
        /// </summary>
        [GkhConfigProperty(DisplayName = "Логин")]
        [DefaultValue("techbars")]
        public virtual string Login { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        [GkhConfigProperty(DisplayName = "Пароль")]
        [DefaultValue("mykazan15")]
        public virtual string Password { get; set; }
    }
}