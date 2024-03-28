namespace Bars.Gkh.Entities.RealEstateType
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Параметр очередности типа дома
    /// </summary>
    public class RealEstateTypePriorityParam : BaseImportableEntity
    {
        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Вес
        /// </summary>
        public virtual int Weight { get; set; }

        /// <summary>
        /// Тип дома
        /// </summary>
        public virtual Dicts.RealEstateType RealEstateType { get; set; }
    }
}