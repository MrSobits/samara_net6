/// <mapping-converter-backup>
/// namespace Bars.Gkh.Gis.Map.GisRealEstate
/// {
///     using B4.DataAccess.ByCode;
///     using Entities.RealEstate.GisRealEstateType;
/// 
///     public class GisRealEstateTypeCommonParamMap : BaseEntityMap<GisRealEstateTypeCommonParam>
///     {
///         public GisRealEstateTypeCommonParamMap()
///             : base("GIS_REAL_EST_TYPE_COMM_PARAM") 
///         {
///             Map(x => x.Min, "MIN", false, 500);
///             Map(x => x.Max, "MAX", false, 500);
///             Map(x => x.CommonParamCode, "COMMON_PARAM_CODE", false, 500);
///             Map(x => x.PrecisionValue, "PRECISION_VALUE", false, 500);
/// 
///             References(x => x.RealEstateType, "REAL_EST_TYPE_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Gis.Map.RealEstate.GisRealEstateType
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Gis.Entities.RealEstate.GisRealEstateType;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Gis.Entities.RealEstate.GisRealEstateType.GisRealEstateTypeCommonParam"</summary>
    public class GisRealEstateTypeCommonParamMap : BaseEntityMap<GisRealEstateTypeCommonParam>
    {
        
        public GisRealEstateTypeCommonParamMap() : 
                base("Bars.Gkh.Gis.Entities.RealEstate.GisRealEstateType.GisRealEstateTypeCommonParam", "GIS_REAL_EST_TYPE_COMM_PARAM")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.CommonParamCode, "CommonParamCode").Column("COMMON_PARAM_CODE").Length(500);
            Reference(x => x.RealEstateType, "RealEstateType").Column("REAL_EST_TYPE_ID").NotNull().Fetch();
            Property(x => x.Min, "Min").Column("MIN").Length(500);
            Property(x => x.Max, "Max").Column("MAX").Length(500);
            Property(x => x.PrecisionValue, "PrecisionValue").Column("PRECISION_VALUE").Length(500);
        }
    }
}
