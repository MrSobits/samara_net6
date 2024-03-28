namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities.Dicts;

    using Gkh.Entities;

    /// <summary>
    /// сущность связи между подтематикой обращения и характеристикой нарушения
    /// </summary>
    public class StatSubsubjectFeatureGji : BaseGkhEntity
    {
        /// <summary>
        /// Подтематика
        /// </summary>
        public virtual StatSubsubjectGji Subsubject { get; set; }

        /// <summary>
        /// Характеристика нарушения
        /// </summary>
        public virtual FeatureViolGji FeatureViol { get; set; }
    }
}