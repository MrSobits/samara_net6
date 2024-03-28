namespace Bars.Gkh.Overhaul.Nso.ConfigSections.OverhaulNso
{
    using System.ComponentModel;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.ConfigSections.Overhaul.Enums;

    /// <summary>
    /// Субсидирование
    /// </summary>
    public class SubsidyConfig : IGkhConfigSection
    {
        /// <summary>
        /// Фиксация опубликованного года
        /// </summary>
        [GkhConfigProperty(DisplayName = "Фиксация опубликованного года")]
        [DefaultValue(TypeUsage.NoUsed)]
        public virtual TypeUsage UseFixationPublishedYears { get; set; }

        /// <summary>
        /// Способ корректировки
        /// </summary>
        [GkhConfigProperty(DisplayName = "Способ корректировки")]
        [DefaultValue(TypeCorrection.WithoutFixYear)]
        public virtual TypeCorrection TypeCorrection { get; set; }

        /// <summary>
        /// Учет плановой собираемости
        /// </summary>
        [GkhConfigProperty(DisplayName = "Учет плановой собираемости")]
        [DefaultValue(UsePlanOwnerCollectionType.LastYear)]
        public virtual UsePlanOwnerCollectionType UsePlanOwnerCollectionType { get; set; }

        /// <summary>
        /// Учет срока эксплуатации КЭ
        /// </summary>
        [GkhConfigProperty(DisplayName = "Учет срока эксплуатации КЭ")]
        [DefaultValue(TypeUsage.NoUsed)]
        public virtual TypeUsage UseLifetime { get; set; }

        /// <summary>
        /// Удаление работ с годом ремонта больше года окончания программы
        /// </summary>
        [GkhConfigProperty(DisplayName = "Удаление работ с годом ремонта больше года окончания программы")]
        [DefaultValue(TypeUsage.NoUsed)]
        public virtual TypeUsage UseDeleteWorkWithGreaterYear { get; set; }
    }
}