namespace Bars.Gkh.Entities.RealEstateType
{
    using Bars.Gkh.Entities;

    using Entities;
    using Dicts;

    /// <summary>
    /// Связка типа дома - дом
    /// </summary>
    public class RealEstateTypeRealityObject : BaseImportableEntity
    {
        /// <summary>
        /// Тип дома
        /// </summary>
        public virtual RealEstateType RealEstateType { get; set; }
 
        /// <summary>
        /// Дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }
    }
}