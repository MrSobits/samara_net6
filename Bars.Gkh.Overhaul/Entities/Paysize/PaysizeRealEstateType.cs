namespace Bars.Gkh.Overhaul.Entities
{
    using Bars.Gkh.Entities;

    using Gkh.Entities.Dicts;

    /// <summary>
    /// Тип дома размера взноса на кр
    /// </summary>
    public class PaysizeRealEstateType : BaseImportableEntity
    {
        /// <summary>
        /// Размер взноса на кр
        /// </summary>
        public virtual PaysizeRecord Record { get; set; }

        /// <summary>
        /// Тип домов
        /// </summary>
        public virtual RealEstateType RealEstateType { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        public virtual decimal? Value { get; set; }
    }
}