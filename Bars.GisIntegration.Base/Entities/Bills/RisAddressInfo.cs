namespace Bars.GisIntegration.Base.Entities.Bills
{
    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Адресные сведения
    /// </summary>
    public class RisAddressInfo : BaseRisEntity
    {
        /// <summary>
        /// Количество проживающих
        /// </summary>
        public virtual byte? LivingPersonsNumber { get; set; }

        /// <summary>
        /// Жилая площадь
        /// </summary>
        public virtual decimal? ResidentialSquare { get; set; }

        /// <summary>
        /// Отапливаемая площадь
        /// </summary>
        public virtual decimal? HeatedArea { get; set; }

        /// <summary>
        /// Общая площадь
        /// </summary>
        public virtual decimal TotalSquare { get; set; }
    }
}