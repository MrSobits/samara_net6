namespace Bars.Gkh.Overhaul.Entities
{
    using Bars.Gkh.Entities;

    using Gkh.Entities.CommonEstateObject;

    /// <summary>
    /// Атрибут конструктивного элемента жилого дома
    /// </summary>
    public class RealityObjectStructuralElementAttributeValue : BaseImportableEntity
    {
        /// <summary>
        /// Атрибут
        /// </summary>
        public virtual StructuralElementGroupAttribute Attribute { get; set; }

        /// <summary>
        /// КЭ жилого дома
        /// </summary>
        public virtual RealityObjectStructuralElement Object { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        public virtual string Value { get; set; }
    }
}