namespace Bars.Gkh.Overhaul.Hmao.ConfigSections.OverhaulHmao
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
        /// Учет краткосрочной программы
        /// </summary>
        [GkhConfigProperty(DisplayName = "Учет краткосрочной программы")]
        [DefaultValue(TypeUsage.NoUsed)]
        public virtual TypeUsage ActualizeUseValidShortProgram { get; set; }

        /// <summary>
        /// Перенос отдельных КЭ в версии
        /// </summary>
        [GkhConfigProperty(DisplayName = "Перенос отдельных КЭ в версии")]
        [DefaultValue(TypeUsage.NoUsed)]
        public virtual TypeUsage TransferSingleStructEl { get; set; }

        /// <summary>
        /// Учет краткосрочной программы при актуализации стоимости
        /// </summary>
        [GkhConfigProperty(DisplayName = "Учет краткосрочной программы при актуализации стоимости")]
        [DefaultValue(TypeUsage.NoUsed)]
        public virtual TypeUsage TransferSingleStructElAtCostUpdating { get; set; }

        /// <summary>
        /// Выборочная актуализация
        /// </summary>
        [GkhConfigProperty(DisplayName = "Выборочная актуализация")]
        [DefaultValue(TypeUsage.NoUsed)]
        public virtual TypeUsage UseSelectiveActualize { get; set; }

        /// <summary>
        /// Актуализация на основе КПКР
        /// </summary>
        [GkhConfigProperty(DisplayName = "Актуализация на основе КПКР")]
        public virtual ActualizeFromCrConfig ActualizeFromCrConfig { get; set; }
    }
}