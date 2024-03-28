namespace Bars.Gkh.Overhaul.Hmao.ConfigSections.OverhaulHmao
{
    using System.ComponentModel;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.ConfigSections.Overhaul.Enums;

    /// <summary>
    /// Актуализация ДПКР
    /// </summary>
    public class ActualizeFromCrConfig : IGkhConfigSection
    {
        /// <summary>
        /// Обновление объема и стоимости
        /// </summary>
        [GkhConfigProperty(DisplayName = "Обновление объема и стоимости")]
        [DefaultValue(TypeUsage.NoUsed)]
        public virtual TypeUpdate TypeSumAndVolumeUpdate { get; set; }

        /// <summary>
        /// Пересчет последующих вхождений записей
        /// </summary>
        [GkhConfigProperty(DisplayName = "Пересчет последующих вхождений записей")]
        [DefaultValue(TypeUsage.Used)]
        public virtual TypeUsage RecalcOtherEntries { get; set; }
    }
}