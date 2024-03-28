namespace Bars.Gkh.Overhaul.Nso.ConfigSections.OverhaulNso
{
    using System.ComponentModel;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.ConfigSections.Overhaul.Enums;

    /// <summary>
    /// Опубликованная программа
    /// </summary>
    public class PublishProgramConfig : IGkhConfigSection
    {
        /// <summary>
        /// Период для публикации (год)
        /// </summary>
        [GkhConfigProperty(DisplayName = "Период для публикации (год)")]
        [DefaultValue(TypeUsage.NoUsed)]
        public virtual int PublicationPeriod { get; set; }

        /// <summary>
        /// Период краткосрочной программы
        /// </summary>
        [GkhConfigProperty(DisplayName = "Период краткосрочной программы")]
        [DefaultValue(TypeUseShortProgramPeriod.Given)]
        public virtual TypeUseShortProgramPeriod UseShortProgramPeriod { get; set; }
    }
}