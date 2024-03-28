namespace Bars.Gkh.Gis.ConfigSections
{
    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Config.Attributes.UI;

    /// <summary>
    /// Настройки ЕГСО ОВ
    /// </summary>
    [GkhConfigSection("Egso", DisplayName = "ЕГСО ОВ")]
    [Permissionable]
    public class EgsoConfig : IGkhConfigSection
    {
        /// <summary>
        /// Настройки интеграции с ЕГСО ОВ
        /// </summary>
        [GkhConfigProperty(DisplayName = "Настройки интеграции с ЕГСО ОВ")]
        [Navigation]
        public virtual EgsoIntegrationConfig EgsoIntegrationConfig { get; set; }
    }
}