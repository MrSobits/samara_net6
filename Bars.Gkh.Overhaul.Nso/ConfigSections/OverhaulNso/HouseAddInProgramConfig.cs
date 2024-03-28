namespace Bars.Gkh.Overhaul.Nso.ConfigSections.OverhaulNso
{
    using System.ComponentModel;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.ConfigSections.Overhaul.Enums;

    /// <summary>
    /// Ограничения добавления домов в программу
    /// </summary>
    public class HouseAddInProgramConfig : IGkhConfigSection
    {
        /// <summary>
        /// Минимальное количество квартир
        /// </summary>
        [GkhConfigProperty(DisplayName = "Минимальное количество квартир")]
        public virtual int MinimumCountApartments { get; set; }

        /// <summary>
        /// Использование износа по основным КЭ
        /// </summary>
        [GkhConfigProperty(DisplayName = "Использование износа по основным КЭ")]
        [DefaultValue(TypeUseWearMainCeo.NotUsed)]
        public virtual TypeUseWearMainCeo TypeUseWearMainCeo { get; set; }

        /// <summary>
        /// Износ основных КЭ
        /// </summary>
        [GkhConfigProperty(DisplayName = "Износ основных КЭ")]
        public virtual decimal WearMainCeo { get; set; }

        /// <summary>
        /// Использование износа по основным КЭ
        /// </summary>
        [GkhConfigProperty(DisplayName = "Использовать физический износ дома")]
        [DefaultValue(TypeUsage.NoUsed)]
        public virtual TypeUsage UsePhysicalWearout { get; set; }

        /// <summary>
        /// Физический износ дома (%)
        /// </summary>
        [GkhConfigProperty(DisplayName = "Физический износ дома (%)")]
        public virtual decimal PhysicalWear { get; set; }

        /// <summary>
        /// Учет предельной стоимости работ
        /// </summary>
        [GkhConfigProperty(DisplayName = "Учет предельной стоимости работ")]
        [DefaultValue(TypeUsage.NoUsed)]
        public virtual TypeUsage UseLimitCost { get; set; }
    }
}