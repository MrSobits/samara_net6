namespace Bars.GkhGji.ConfigSections
{
    using Bars.Gkh.Config;
    using Bars.GkhGji.ConfigSections.SettingsOfTheDay;

    using Gkh.Config.Attributes;
    using Gkh.Config.Attributes.UI;

    /// <summary>
    /// Жилищная инспекция
    /// </summary>
    [GkhConfigSection("HousingInspection", DisplayName = "Жилищная инспекция")]
    [Permissionable]
    public class HousingInspection : IGkhConfigSection
    {
        /// <inheritdoc />
        [GkhConfigProperty(DisplayName = "Общие")]
        [Navigation]
        [Permissionable]
        public virtual GeneralConfig GeneralConfig { get; set; }

        /// <inheritdoc />
        [GkhConfigProperty(DisplayName = "Настройки рабочего дня")]
        [Navigation]
        public virtual SettingsOfTheDayConfig SettingsOfTheDay { get; set; }

        /// <inheritdoc />
        [GkhConfigProperty(DisplayName = "Настройки продолжительности проверки")]
        [Navigation]
        public virtual SettingTheVerificationConfig SettingTheVerification { get; set; }
    }
}
