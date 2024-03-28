namespace Bars.Gkh.Gis.DomainService.Indicator
{
    using System.Collections.Generic;

    using Bars.Gkh.Entities.Dicts;

    using Enum;

    public class IndicatorGroupProxy
    {
        public ServiceDictionary Service { get; set; }
        /// <summary>
        /// Key - индикатор, Value - его Id в таблице сопоставления
        /// </summary>
        public Dictionary<GisTypeIndicator, long> Indicators { get; set; }
    }
}