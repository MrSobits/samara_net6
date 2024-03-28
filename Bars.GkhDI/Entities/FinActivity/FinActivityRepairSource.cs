namespace Bars.GkhDi.Entities
{
    using Bars.Gkh.Entities;
    using Bars.GkhDi.Enums;

    /// <summary>
    /// Объем привлеченных средств на ремонт и благоустройство
    /// </summary>
    public class FinActivityRepairSource : BaseGkhEntity
    {
        /// <summary>
        /// Сведения об УО
        /// </summary>
        public virtual DisclosureInfo DisclosureInfo { get; set; }

        /// <summary>
        /// Тип источника упр орг
        /// </summary>
        public virtual TypeSourceFundsDi TypeSourceFundsDi { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public virtual decimal? Sum { get; set; }

        /// <summary>
        /// Признак не валидности для саммари(не хранимое)
        /// </summary>
        public virtual string IsInvalid { get; set; }
    }
}
