/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Nso.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class VersionRecordStage2Map : BaseEntityMap<VersionRecordStage2>
///     {
///         public VersionRecordStage2Map()
///             : base("OVRHL_STAGE2_VERSION")
///         {
///             Map(x => x.CommonEstateObjectWeight, "CE_WEIGHT", true, 0);
///             Map(x => x.Sum, "SUM", true, 0);
/// 
///             References(x => x.Stage3Version, "ST3_VERSION_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.CommonEstateObject, "COMMON_ESTATE_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Nso.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Nso.Entities.VersionRecordStage2"</summary>
    public class VersionRecordStage2Map : BaseEntityMap<VersionRecordStage2>
    {
        
        public VersionRecordStage2Map() : 
                base("Bars.Gkh.Overhaul.Nso.Entities.VersionRecordStage2", "OVRHL_STAGE2_VERSION")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Stage3Version, "Stage3Version").Column("ST3_VERSION_ID").NotNull().Fetch();
            Property(x => x.CommonEstateObjectWeight, "CommonEstateObjectWeight").Column("CE_WEIGHT").NotNull();
            Property(x => x.Sum, "Sum").Column("SUM").NotNull();
            Reference(x => x.CommonEstateObject, "CommonEstateObject").Column("COMMON_ESTATE_ID").NotNull().Fetch();
        }
    }
}
