namespace Bars.GkhGji.ConfigSections.SettingsOfTheDay
{
    using System.ComponentModel;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;

    /// <summary>
    /// Настройки продолжительности проверки
    /// </summary>
    public class SettingTheVerificationConfig : IGkhConfigSection
    {
        /// <summary>
        /// Конфигурации субъект среднего бизнеса 
        /// </summary>
        [GkhConfigProperty(DisplayName = "Субъект среднего бизнеса")]
        public virtual SubjectBusinessConfig SubjectMediumBusinessConfig { get; set; }

        /// <summary>
        /// Конфигурации субъект малого бизнеса
        /// </summary>
        [GkhConfigProperty(DisplayName = "Субъект малого бизнеса")]
        public virtual SubjectBusinessConfig SubjectBusinessConfig { get; set; }

        /// <summary>
        /// Конфигурации субъект микро бизнеса
        /// </summary>
        [GkhConfigProperty(DisplayName = "Субъект микро бизнеса")]
        public virtual SubjectBusinessConfig SubjectsMicrobusinessesConfig { get; set; }

        /// <summary>
        /// Автоматическре проставление срока исполнения
        /// </summary>
        [GkhConfigProperty(DisplayName = "Автоматическое проставление Срока исполнения")]
        [DefaultValue(false)]
        public virtual bool AutoPerformanceDate { get; set; }
    }
}