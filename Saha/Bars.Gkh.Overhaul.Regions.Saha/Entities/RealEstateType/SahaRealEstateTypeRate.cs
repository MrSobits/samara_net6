namespace Bars.Gkh.Overhaul.Regions.Saha.Entities.RealEstateType
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// Тариф по типам домов
    /// </summary>
    public class SahaRealEstateTypeRate : BaseEntity
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
        /// Социально допустимый (установочный) тариф, руб./кв.м (нежилая площадь)
        /// </summary>
        public virtual decimal? SociallyAcceptableRateNotLivingArea { get; set; }

        /// <summary>
        /// Потребность в финансировании, руб
        /// </summary>
        public virtual decimal? NeedForFunding { get; set; }

        /// <summary>
        /// Площадь помещений, кв.м
        /// </summary>
        public virtual decimal? TotalArea { get; set; }

        /// <summary>
        /// Площадь помещений, кв.м (нежилая площадь)
        /// </summary>
        public virtual decimal? TotalNotLivingArea { get; set; }

        /// <summary>
        /// Экономически обоснованный тариф, руб./кв.м.
        /// </summary>
        public virtual decimal? ReasonableRate { get; set; }

        /// <summary>
        /// Экономически обоснованный тариф, руб./кв.м. (нежилая площадь)
        /// </summary>
        public virtual decimal? ReasonableRateNotLivingArea { get; set; }

        /// <summary>
        /// Дефицит тарифа, руб./кв.м.
        /// </summary>
        public virtual decimal? RateDeficit { get; set; }

        /// <summary>
        /// Дефицит тарифа, руб./кв.м. (нежилая площадь)
        /// </summary>
        public virtual decimal? RateDeficitNotLivingArea { get; set; }

        /// <summary>
        /// Год
        /// </summary>
        public virtual int Year { get; set; }
    }
}