namespace Bars.Gkh.Overhaul.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities.Dicts;

    using Gkh.Entities.CommonEstateObject;

    /// <summary>
    /// Характеристики нарушений
    /// </summary>
    public class StructuralElementFeatureViol : BaseEntity
    {
        /// <summary>
        /// Конструктивный элемент
        /// </summary>
        public virtual StructuralElement StructuralElement { get; set; }

        /// <summary>
        /// Группа нарушений
        /// </summary>
        public virtual FeatureViolGji FeatureViol { get; set; }
    }
}