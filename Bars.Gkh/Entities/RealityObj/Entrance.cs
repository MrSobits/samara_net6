namespace Bars.Gkh.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Подъезд
    /// </summary>
    public class Entrance : BaseImportableEntity
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Номер подъезда
        /// </summary>
        public virtual int Number { get; set; }

        /// <summary>
        /// Тип дома
        /// </summary>
        public virtual Dicts.RealEstateType RealEstateType { get; set; }
    }
}