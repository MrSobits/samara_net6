/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Tat.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.Gkh.Overhaul.Tat.Enum;
///     using Entities;
/// 
///     public class VersionRecordStage2Map : BaseEntityMap<VersionRecordStage2>
///     {
///         public VersionRecordStage2Map()
///             : base("OVRHL_STAGE2_VERSION")
///         {
///             Map(x => x.CommonEstateObjectWeight, "CE_WEIGHT").Not.Nullable();
///             Map(x => x.Sum, "SUM").Not.Nullable();
/// 
///             Map(x => x.TypeDpkrRecord, "TYPE_DPKR_RECORD").Not.Nullable().CustomType<TypeDpkrRecord>();
///             Map(x => x.TypeCorrection, "TYPE_CORRECTION").Not.Nullable().CustomType<TypeCorrection>();
/// 
///             References(x => x.Stage3Version, "ST3_VERSION_ID").Not.Nullable().Fetch.Join();
///             References(x => x.CommonEstateObject, "COMMON_ESTATE_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Tat.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Tat.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Tat.Entities.VersionRecordStage2"</summary>
    public class VersionRecordStage2Map : BaseEntityMap<VersionRecordStage2>
    {
        
        public VersionRecordStage2Map() : 
                base("Bars.Gkh.Overhaul.Tat.Entities.VersionRecordStage2", "OVRHL_STAGE2_VERSION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.CommonEstateObjectWeight, "CommonEstateObjectWeight").Column("CE_WEIGHT").NotNull();
            Property(x => x.Sum, "Sum").Column("SUM").NotNull();
            Property(x => x.TypeDpkrRecord, "TypeDpkrRecord").Column("TYPE_DPKR_RECORD").NotNull();
            Property(x => x.TypeCorrection, "TypeCorrection").Column("TYPE_CORRECTION").NotNull();
            Reference(x => x.Stage3Version, "Stage3Version").Column("ST3_VERSION_ID").NotNull().Fetch();
            Reference(x => x.CommonEstateObject, "CommonEstateObject").Column("COMMON_ESTATE_ID").NotNull().Fetch();
        }
    }
}
