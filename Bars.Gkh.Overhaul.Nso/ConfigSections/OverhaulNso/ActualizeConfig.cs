namespace Bars.Gkh.Overhaul.Nso.ConfigSections.OverhaulNso
{
    using System.ComponentModel;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.ConfigSections.Overhaul.Enums;

    /// <summary>
    /// Актуализация ДПКР
    /// </summary>
    public class ActualizeConfig : IGkhConfigSection
    {
        /// <summary>
        /// Проверка наличия краткосрочной программы
        /// </summary>
        [GkhConfigProperty(DisplayName = "Проверка наличия краткосрочной программы")]
        [DefaultValue(TypeUsage.NoUsed)]
        public virtual TypeUsage ActualizeUseValidShortProgram { get; set; }
    }
}