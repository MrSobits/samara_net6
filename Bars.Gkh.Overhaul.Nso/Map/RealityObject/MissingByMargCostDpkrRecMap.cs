/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Nso.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Nso.Entities;
/// 
///     public class MissingByMargCostDpkrRecMap : BaseEntityMap<MissingByMargCostDpkrRec>
///     {
///         public MissingByMargCostDpkrRecMap()
///             : base("OVRHL_MISS_DPKR_REC")
///         {
///             References(x => x.RealityObject, "RO_ID", ReferenceMapConfig.NotNullAndFetch);
///             Map(x => x.Year, "YEAR", true, 0);
///             Map(x => x.Sum, "SUM", true, 0);
///             Map(x => x.CommonEstateObjects, "CEO_STRING", true);
///             Map(x => x.RealEstateTypeName, "REAL_EST_TYPE_NAME", true);
///             Map(x => x.MargSum, "MARG_REPAIR_COST");
///             Map(x => x.Area, "AREA_MKD");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Nso.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Nso.Entities.MissingByMargCostDpkrRec"</summary>
    public class MissingByMargCostDpkrRecMap : BaseEntityMap<MissingByMargCostDpkrRec>
    {
        
        public MissingByMargCostDpkrRecMap() : 
                base("Bars.Gkh.Overhaul.Nso.Entities.MissingByMargCostDpkrRec", "OVRHL_MISS_DPKR_REC")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.RealityObject, "RealityObject").Column("RO_ID").NotNull().Fetch();
            Property(x => x.Year, "Year").Column("YEAR").NotNull();
            Property(x => x.CommonEstateObjects, "CommonEstateObjects").Column("CEO_STRING").Length(250).NotNull();
            Property(x => x.Sum, "Sum").Column("SUM").NotNull();
            Property(x => x.Area, "Area").Column("AREA_MKD");
            Property(x => x.MargSum, "MargSum").Column("MARG_REPAIR_COST");
            Property(x => x.RealEstateTypeName, "RealEstateTypeName").Column("REAL_EST_TYPE_NAME").Length(250).NotNull();
        }
    }
}
