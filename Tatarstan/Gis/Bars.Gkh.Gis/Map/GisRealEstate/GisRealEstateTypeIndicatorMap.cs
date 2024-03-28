/// <mapping-converter-backup>
/// namespace Bars.Gkh.Gis.Map.GisRealEstate
/// {
///     using B4.DataAccess.ByCode;
///     using Entities.RealEstate.GisRealEstateType;
/// 
///     public class GisRealEstateTypeIndicatorMap : BaseEntityMap<GisRealEstateTypeIndicator>
///     {
///         public GisRealEstateTypeIndicatorMap()
///             : base("GIS_REAL_EST_TYPE_INDICATOR") 
///         {
///             Map(x => x.Min, "MIN", false, 500);
///             Map(x => x.Max, "MAX", false, 500);
///             Map(x => x.PrecisionValue, "PRECISION_VALUE", false, 500);
/// 
///             References(x => x.RealEstateType, "REAL_EST_TYPE_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.RealEstateIndicator, "REAL_EST_INDICATOR_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Gis.Map.RealEstate.GisRealEstateType
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Gis.Entities.RealEstate.GisRealEstateType;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Gis.Entities.RealEstate.GisRealEstateType.GisRealEstateTypeIndicator"</summary>
    public class GisRealEstateTypeIndicatorMap : BaseEntityMap<GisRealEstateTypeIndicator>
    {
        
        public GisRealEstateTypeIndicatorMap() : 
                base("Bars.Gkh.Gis.Entities.RealEstate.GisRealEstateType.GisRealEstateTypeIndicator", "GIS_REAL_EST_TYPE_INDICATOR")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.RealEstateIndicator, "RealEstateIndicator").Column("REAL_EST_INDICATOR_ID").NotNull().Fetch();
            Reference(x => x.RealEstateType, "RealEstateType").Column("REAL_EST_TYPE_ID").NotNull().Fetch();
            Property(x => x.Min, "Min").Column("MIN").Length(500);
            Property(x => x.Max, "Max").Column("MAX").Length(500);
            Property(x => x.PrecisionValue, "PrecisionValue").Column("PRECISION_VALUE").Length(500);
        }
    }
}
