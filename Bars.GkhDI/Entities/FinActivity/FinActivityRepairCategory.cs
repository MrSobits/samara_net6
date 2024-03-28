namespace Bars.GkhDi.Entities
{
    using Bars.Gkh.Entities;
    using Bars.GkhDi.Enums;

    /// <summary>
    /// Ремонт дома и благоустройство территории, средний срок обслуживания МКД
    /// </summary>
    public class FinActivityRepairCategory : BaseGkhEntity
    {
        /// <summary>
        /// Сведения об УО
        /// </summary>
        public virtual DisclosureInfo DisclosureInfo { get; set; }

        /// <summary>
        /// Тип категории дома
        /// </summary>
        public virtual TypeCategoryHouseDi TypeCategoryHouseDi { get; set; }

        /// <summary>
        /// Объем работ по ремонту
        /// </summary>
        public virtual decimal? WorkByRepair { get; set; }

        /// <summary>
        /// Объем работ по благоустройству
        /// </summary>
        public virtual decimal? WorkByBeautification { get; set; }

        /// <summary>
        /// Срок обслуживания
        /// </summary>
        public virtual int? PeriodService { get; set; }

        /// <summary>
        /// Признак не валидности для саммари(не хранимое)
        /// </summary>
        public virtual string IsInvalid { get; set; }
    }
}
