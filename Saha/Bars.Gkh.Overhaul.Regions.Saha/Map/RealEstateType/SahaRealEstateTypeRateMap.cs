/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Regions.Saha.Map.RealEstateType
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Regions.Saha.Entities.RealEstateType;
/// 
///     public class SahaRealEstateTypeRateMap : BaseEntityMap<SahaRealEstateTypeRate>
///     {
///         public SahaRealEstateTypeRateMap()
///             : base("OVRHL_REAL_EST_TYPE_RATE")
///         {
///             Map(x => x.SociallyAcceptableRate, "SOCIALLY_ACCEPTABLE_RATE");
///             Map(x => x.SociallyAcceptableRateNotLivingArea, "SOCIALLY_ACCEPTABLE_RATE_NOTLIV");
///             Map(x => x.NeedForFunding, "NEED_FOR_FUNDING");
///             Map(x => x.TotalArea, "TOTAL_AREA");
///             Map(x => x.TotalNotLivingArea, "TOTAL_AREA_NOTLIV");
///             Map(x => x.ReasonableRate, "REASONABLE_RATE");
///             Map(x => x.ReasonableRateNotLivingArea, "REASONABLE_RATE_NOTLIV");
///             Map(x => x.RateDeficit, "RATE_DEFICIT");
///             Map(x => x.RateDeficitNotLivingArea, "RATE_DEFICIT_NOTLIV");
///             Map(x => x.Year, "YEAR");
/// 
///             References(x => x.RealEstateType, "REAL_ESTATE_TYPE_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Regions.Saha.Map.RealEstateType
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Regions.Saha.Entities.RealEstateType;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Regions.Saha.Entities.RealEstateType.SahaRealEstateTypeRate"</summary>
    public class SahaRealEstateTypeRateMap : BaseEntityMap<SahaRealEstateTypeRate>
    {
        
        public SahaRealEstateTypeRateMap() : 
                base("Bars.Gkh.Overhaul.Regions.Saha.Entities.RealEstateType.SahaRealEstateTypeRate", "OVRHL_REAL_EST_TYPE_RATE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.RealEstateType, "RealEstateType").Column("REAL_ESTATE_TYPE_ID").Fetch();
            Property(x => x.SociallyAcceptableRate, "SociallyAcceptableRate").Column("SOCIALLY_ACCEPTABLE_RATE");
            Property(x => x.SociallyAcceptableRateNotLivingArea, "SociallyAcceptableRateNotLivingArea").Column("SOCIALLY_ACCEPTABLE_RATE_NOTLIV");
            Property(x => x.NeedForFunding, "NeedForFunding").Column("NEED_FOR_FUNDING");
            Property(x => x.TotalArea, "TotalArea").Column("TOTAL_AREA");
            Property(x => x.TotalNotLivingArea, "TotalNotLivingArea").Column("TOTAL_AREA_NOTLIV");
            Property(x => x.ReasonableRate, "ReasonableRate").Column("REASONABLE_RATE");
            Property(x => x.ReasonableRateNotLivingArea, "ReasonableRateNotLivingArea").Column("REASONABLE_RATE_NOTLIV");
            Property(x => x.RateDeficit, "RateDeficit").Column("RATE_DEFICIT");
            Property(x => x.RateDeficitNotLivingArea, "RateDeficitNotLivingArea").Column("RATE_DEFICIT_NOTLIV");
            Property(x => x.Year, "Year").Column("YEAR");
        }
    }
}
