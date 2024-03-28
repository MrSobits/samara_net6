/// <mapping-converter-backup>
/// namespace Bars.Gkh.Gis.Map.IndicatorServiceComparison
/// {
///     using B4.DataAccess.ByCode;
///     using Entities.IndicatorServiceComparison;
/// 
///     public class IndicatorGroupingMap : BaseEntityMap<IndicatorServiceComparison>
///     {
///         public IndicatorGroupingMap()
///             : base("GIS_INDICATOR_GROUPING")
///         {
///             Map(x => x.TypeGroupIndicators, "TYPE_GROUP_INDICATORS", true);
///             Map(x => x.TypeIndicator, "TYPE_INDICATORS", true);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Gis.Map.IndicatorServiceComparison
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Gis.Entities.IndicatorServiceComparison;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Gis.Entities.IndicatorServiceComparison.IndicatorServiceComparison"</summary>
    public class IndicatorServiceMap : BaseEntityMap<IndicatorServiceComparison>
    {

        public IndicatorServiceMap() : 
                base("Bars.Gkh.Gis.Entities.IndicatorServiceComparison.IndicatorServiceComparison", "GIS_INDICATOR_SERVICE_COMP")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Service, "Service").Column("SERVICE").Fetch();
            Property(x => x.GisTypeIndicator, "TypeIndicator").Column("TYPE_INDICATORS").NotNull();
        }
    }
}