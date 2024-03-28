/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map.RealEstateType
/// {
///     using B4.DataAccess.ByCode;
///     using Entities.RealEstateType;
/// 
///     public class RealEstateTypeRateMap : BaseImportableEntityMap<RealEstateTypeRate>
///     {
///         public RealEstateTypeRateMap()
///             : base("OVRHL_REAL_EST_TYPE_RATE")
///         {
///             Map(x => x.SociallyAcceptableRate, "SOCIALLY_ACCEPTABLE_RATE");
///             Map(x => x.NeedForFunding, "NEED_FOR_FUNDING");
///             Map(x => x.TotalArea, "TOTAL_AREA");
///             Map(x => x.ReasonableRate, "REASONABLE_RATE");
///             Map(x => x.RateDeficit, "RATE_DEFICIT");
///             Map(x => x.Year, "YEAR");
/// 
///             References(x => x.RealEstateType, "REAL_ESTATE_TYPE_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map.RealEstateType
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.RealEstateType;
    
    
    /// <summary>Маппинг для "Тариф по типам домов"</summary>
    public class RealEstateTypeRateMap : BaseImportableEntityMap<RealEstateTypeRate>
    {
        
        public RealEstateTypeRateMap() : 
                base("Тариф по типам домов", "OVRHL_REAL_EST_TYPE_RATE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.RealEstateType, "Тип дома").Column("REAL_ESTATE_TYPE_ID").Fetch();
            Property(x => x.SociallyAcceptableRate, "Социально допустимый (установочный) тариф, руб./кв.м").Column("SOCIALLY_ACCEPTABLE_RATE");
            Property(x => x.NeedForFunding, "Потребность в финансировании, руб").Column("NEED_FOR_FUNDING");
            Property(x => x.TotalArea, "Жилая площадь, кв.м").Column("TOTAL_AREA");
            Property(x => x.ReasonableRate, "Экономически обоснованный тариф, руб./кв.м.").Column("REASONABLE_RATE");
            Property(x => x.RateDeficit, "Дефицит тарифа, руб./кв.м.").Column("RATE_DEFICIT");
            Property(x => x.Year, "Год").Column("YEAR");
        }
    }
}
