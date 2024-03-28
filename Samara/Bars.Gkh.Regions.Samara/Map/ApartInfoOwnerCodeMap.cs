/// <mapping-converter-backup>
/// namespace Bars.Gkh.Regions.Samara.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Regions.Samara.Entities;
/// 
///     public class ApartInfoOwnerCodeMap : BaseEntityMap<ApartInfoOwnerCode>
///     {
///         public ApartInfoOwnerCodeMap()
///             : base("GKH_SAM_AP_INF_OWN_CODE")
///         {
///             Map(x => x.OwnerCode, "OWNER_CODE");
///             References(x => x.RealityObjectApartInfo, "AP_INFO_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Regions.Samara.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Samara.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Regions.Samara.Entities.ApartInfoOwnerCode"</summary>
    public class ApartInfoOwnerCodeMap : BaseEntityMap<ApartInfoOwnerCode>
    {
        
        public ApartInfoOwnerCodeMap() : 
                base("Bars.Gkh.Regions.Samara.Entities.ApartInfoOwnerCode", "GKH_SAM_AP_INF_OWN_CODE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.RealityObjectApartInfo, "RealityObjectApartInfo").Column("AP_INFO_ID").NotNull().Fetch();
            Property(x => x.OwnerCode, "OwnerCode").Column("OWNER_CODE").Length(250);
        }
    }
}
