namespace Bars.GkhGji.ConfigSections
{
    using Gkh.Config;
    using Gkh.Config.Attributes;
    using System.ComponentModel;
    using Enums;

    /// <summary>
    /// Субъект бизнеса
    /// </summary>
    public class SubjectBusinessConfig : IGkhConfigSection
    {
        /// <summary>
        /// Продолжительность проверки
        /// </summary>
        [GkhConfigProperty(DisplayName = "Продолжительность проверки")]
        public virtual int Duration { get; set; }

        /// <summary>
        /// Настройки продолжительности проверки
        /// </summary>
        [GkhConfigProperty(DisplayName = "Единицы измерения")]
        [DefaultValue(Units.Day)]
        public virtual Units Units { get; set; }
    }
}