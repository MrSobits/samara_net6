namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// Сущность связи между Нарушением и Характеристикой нарушения
    /// </summary>
    public class ViolationFeatureGji : BaseGkhEntity
    {
        /// <summary>
        /// Характеристика нарушения
        /// </summary>
        public virtual FeatureViolGji FeatureViolGji { get; set; }

        /// <summary>
        /// Нарушение
        /// </summary>
        public virtual ViolationGji ViolationGji { get; set; }
    }
}