/// <mapping-converter-backup>
/// namespace Bars.Gkh.Gis.Map.Register.MultipleAnalysis
/// {
///     using B4.DataAccess.ByCode;
///     using Entities.Register.MultipleAnalysis;
/// 
///     public class MultipleAnalysisTemplateMap : BaseEntityMap<MultipleAnalysisTemplate>
///     {
///         public MultipleAnalysisTemplateMap()
///             : base("GIS_MULTIPLE_ANALYSIS_TEMPLATE")
///         {
///             References(x => x.RealEstateType, "REAL_ESTATE_TYPE_ID", ReferenceMapConfig.NotNullAndFetch);
///             Map(x => x.TypeCondition, "TYPE_CONDITION", true);
///             Map(x => x.FormDay, "FORMDAY", true);
///             Map(x => x.Email, "EMAIL", true);
///             Map(x => x.LastFormDate, "LAST_FORM_DATE");
///             Map(x => x.MunicipalAreaGuid, "MUNICIPAL_AREA_GUID");
///             Map(x => x.SettlementGuid, "SETTLEMENT_GUID");
///             Map(x => x.StreetGuid, "STREET_GUID");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Gis.Map.Register.MultipleAnalysis
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Gis.Entities.Register.MultipleAnalysis;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Gis.Entities.Register.MultipleAnalysis.MultipleAnalysisTemplate"</summary>
    public class MultipleAnalysisTemplateMap : BaseEntityMap<MultipleAnalysisTemplate>
    {
        
        public MultipleAnalysisTemplateMap() : 
                base("Bars.Gkh.Gis.Entities.Register.MultipleAnalysis.MultipleAnalysisTemplate", "GIS_MULTIPLE_ANALYSIS_TEMPLATE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.RealEstateType, "RealEstateType").Column("REAL_ESTATE_TYPE_ID").NotNull().Fetch();
            Property(x => x.TypeCondition, "TypeCondition").Column("TYPE_CONDITION").NotNull();
            Property(x => x.FormDay, "FormDay").Column("FORMDAY").NotNull();
            Property(x => x.Email, "Email").Column("EMAIL").Length(250).NotNull();
            Property(x => x.LastFormDate, "LastFormDate").Column("LAST_FORM_DATE");
            Property(x => x.MunicipalAreaGuid, "MunicipalAreaGuid").Column("MUNICIPAL_AREA_GUID").Length(250);
            Property(x => x.SettlementGuid, "SettlementGuid").Column("SETTLEMENT_GUID").Length(250);
            Property(x => x.StreetGuid, "StreetGuid").Column("STREET_GUID").Length(250);
            Property(x => x.MonthYear, "MonthYear").Column("MONTH_YEAR");
        }
    }
}