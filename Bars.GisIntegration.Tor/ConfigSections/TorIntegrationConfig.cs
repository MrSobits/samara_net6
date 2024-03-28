namespace Bars.GisIntegration.Tor.ConfigSections
{
    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Config.Attributes.UI;

    /// <summary>
    /// Настройка параметров интеграции с ТОР
    /// </summary>
    [GkhConfigSection("TorIntegrationConfig", DisplayName = "Настройка параметров интеграции с ТОР КНД")]
    [Navigation]
    public class TorIntegrationConfig : IGkhConfigSection
    {
        /// <summary>
        /// Адрес прокси-сервера
        /// </summary>
        [GkhConfigProperty(DisplayName = "Адрес для отправления запросов")]
        public virtual string Address { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        [GkhConfigProperty(DisplayName = "Токен")]
        public virtual string Token { get; set; }
    }
}