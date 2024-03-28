namespace Bars.Gkh.Entities.RealEstateType
{
    using Bars.Gkh.Entities;

    using Dicts;

    /// <summary>
    /// Тариф по типам домов
    /// </summary>
    public class RealEstateTypeRate : BaseImportableEntity
    {
        /// <summary>
        /// Тип дома
        /// </summary>
        public virtual RealEstateType RealEstateType { get; set; }

        /// <summary>
        /// Социально допустимый (установочный) тариф, руб./кв.м
        /// </summary>
        public virtual decimal? SociallyAcceptableRate { get; set; }

        /// <summary>
        /// Потребность в финансировании, руб
        /// </summary>
        public virtual decimal? NeedForFunding { get; set; }

        /// <summary>
        /// Жилая площадь, кв.м
        /// </summary>
        public virtual decimal? TotalArea { get; set; }

        /// <summary>
        /// Экономически обоснованный тариф, руб./кв.м.
        /// </summary>
        public virtual decimal? ReasonableRate { get; set; }

        /// <summary>
        /// Дефицит тарифа, руб./кв.м.
        /// </summary>
        public virtual decimal? RateDeficit { get; set; }

        /// <summary>
        /// Год
        /// </summary>
        public virtual int Year { get; set; }
    }
}