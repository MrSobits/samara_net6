namespace Bars.Gkh.Gis.ConfigSections
{
    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Config.Attributes.UI;

    /// <summary>
    /// Настройки модуля "ГИС"
    /// </summary>
    [GkhConfigSection("Gis", DisplayName = "ГИС")]
    [Permissionable]
    public class GisConfig : IGkhConfigSection
    {
        /// <summary>
        /// Настройки интеграции с Открытым Татарстаном
        /// </summary>
        [GkhConfigProperty(DisplayName = "Настройки интеграции с Открытым Татарстаном")]
        [Navigation]
        public virtual OpenTatarstanIntegrationConfig OpenTatarstanIntegrationConfig { get; set; }

        /// <summary>
        /// Настройки интеграции с Открытой Казанью
        /// </summary>
        [GkhConfigProperty(DisplayName = "Настройки интеграции с Открытой Казанью")]
        [Navigation]
        public virtual OpenKazanIntegrationConfig OpenKazanIntegrationConfig { get; set; }

        /// <summary>
        /// Настройки интеграции с Народным Контролем
        /// </summary>
        [GkhConfigProperty(DisplayName = "Настройки интеграции с Народным Контролем")]
        [Navigation]
        public virtual PublicControlIntegrationConfig PublicControlIntegrationConfig { get; set; }

        /// <summary>
        /// Настройки подключений к БД
        /// </summary>
        [GkhConfigProperty(DisplayName = "Настройки подключений к БД биллинга")]
        [Navigation]
        public virtual BilConnectionConfig BilConnectionConfig { get; set; }

        /// <summary>
        /// Настройки интеграции с ЕИАС
        /// </summary>
        [GkhConfigProperty(DisplayName = "Настройки интеграции с ЕИАС")]
        [Navigation]
        [Permissionable]
        public virtual EiasIntegrationConfig EiasIntegrationConfig { get; set; }
    }
}