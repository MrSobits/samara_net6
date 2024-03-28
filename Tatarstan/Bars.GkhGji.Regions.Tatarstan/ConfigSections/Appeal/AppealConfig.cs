namespace Bars.GkhGji.Regions.Tatarstan.ConfigSections.Appeal
{
    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Config.Attributes.UI;

    /// <summary>
    /// Настройки обращений
    /// </summary>
    [GkhConfigSection("Appeal", DisplayName = "Обращения")]
    [Navigation]
    public class AppealConfig : IGkhConfigSection
    {
        /// <summary>
        /// Настройки СОПР
        /// </summary>
        [GkhConfigProperty(DisplayName = "Настройки СОПР")]
        [Navigation]
        public virtual RapidResponseSystemConfig RapidResponseSystemConfig { get; set; }
    }
}