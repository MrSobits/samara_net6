namespace Bars.GkhGji.Regions.Tatarstan.ConfigSections
{
    using Bars.GkhGji.ConfigSections.SettingsOfTheDay;

    using Gkh.Config.Attributes;
    using Gkh.Config.Attributes.UI;
    using Gkh.Config;
    using Bars.GkhGji.ConfigSections;

    /// <summary>
    /// Жилищная инспекция
    /// </summary>
    [GkhConfigSection("HousingInspection", DisplayName = "Жилищная инспекция")]
    [Permissionable]
    public class HousingInspection : IGkhConfigSection
    {
        /// <summary>
        /// Общие настройки ГЖИ
        /// </summary>
        [GkhConfigProperty(DisplayName = "Общие")]
        [Navigation]
        [Permissionable]
        public virtual GeneralConfig GeneralConfig { get; set; }

        /// <summary>
        /// Настройки рабочего дня
        /// </summary>
        [GkhConfigProperty(DisplayName = "Настройки рабочего дня")]
        [Navigation]
        public virtual SettingsOfTheDayConfig SettingsOfTheDay { get; set; }

        /// <summary>
        /// Настройки продолжительности проверки
        /// </summary>
        [GkhConfigProperty(DisplayName = "Настройки продолжительности проверки")]
        [Navigation]
        public virtual SettingTheVerificationConfig SettingTheVerification { get; set; }
    }
}
