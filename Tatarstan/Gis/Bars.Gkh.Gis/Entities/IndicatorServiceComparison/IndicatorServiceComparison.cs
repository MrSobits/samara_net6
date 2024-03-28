namespace Bars.Gkh.Gis.Entities.IndicatorServiceComparison
{
    using B4.DataAccess;

    using Bars.Gkh.Entities.Dicts;

    using Enum;

    /// <summary>
    /// Сопоставление услуг и индикаторов
    /// </summary>
    public class IndicatorServiceComparison : BaseEntity
    {
        /// <summary>
        /// Услуга
        /// </summary>
        public virtual ServiceDictionary Service { get; set; }

        /// <summary>
        /// Индикатор
        /// </summary>
        public virtual GisTypeIndicator GisTypeIndicator { get; set; }
    }
}