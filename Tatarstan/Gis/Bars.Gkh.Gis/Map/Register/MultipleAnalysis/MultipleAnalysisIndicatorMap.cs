/// <mapping-converter-backup>
/// namespace Bars.Gkh.Gis.Map.Register.MultipleAnalysis
/// {
///     using B4.DataAccess.ByCode;
///     using Entities.Register.MultipleAnalysis;
/// 
///     public class MultipleAnalysisIndicatorMap : BaseEntityMap<MultipleAnalysisIndicator>
///     {
///         public MultipleAnalysisIndicatorMap()
///             : base("GIS_MULTIPLE_ANALYSIS_INDICATOR")
///         {
///             References(x => x.IndicatorServiceComparison, "INDICATOR_GROUPING_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.MultipleAnalysisTemplate, "MULTIPLE_ANALYSIS_TEMPLATE_ID", ReferenceMapConfig.NotNullAndFetch);
///             Map(x => x.MinValue, "MINVALUE");
///             Map(x => x.MaxValue, "MAXVALUE");
///             Map(x => x.DeviationPercent, "DEVIATIONPERCENT");
///             Map(x => x.ExactValue, "EXACTVALUE");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Gis.Map.Register.MultipleAnalysis
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Gis.Entities.Register.MultipleAnalysis;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Gis.Entities.Register.MultipleAnalysis.MultipleAnalysisIndicator"</summary>
    public class MultipleAnalysisIndicatorMap : BaseEntityMap<MultipleAnalysisIndicator>
    {
        
        public MultipleAnalysisIndicatorMap() : 
                base("Bars.Gkh.Gis.Entities.Register.MultipleAnalysis.MultipleAnalysisIndicator", "GIS_MULTIPLE_ANALYSIS_INDICATOR")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.IndicatorServiceComparison, "IndicatorServiceComparison").Column("INDICATOR_GROUPING_ID").NotNull().Fetch();
            Reference(x => x.MultipleAnalysisTemplate, "MultipleAnalysisTemplate").Column("MULTIPLE_ANALYSIS_TEMPLATE_ID").NotNull().Fetch();
            Property(x => x.MinValue, "MinValue").Column("MINVALUE");
            Property(x => x.MaxValue, "MaxValue").Column("MAXVALUE");
            Property(x => x.DeviationPercent, "DeviationPercent").Column("DEVIATIONPERCENT");
            Property(x => x.ExactValue, "ExactValue").Column("EXACTVALUE");
        }
    }
}