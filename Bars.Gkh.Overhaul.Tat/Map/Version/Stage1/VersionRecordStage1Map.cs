/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Tat.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.Gkh.Overhaul.Tat.Entities;
///     using Bars.Gkh.Overhaul.Tat.Enum;
/// 
///     public class VersionRecordStage1Map : BaseEntityMap<VersionRecordStage1>
///     {
///         public VersionRecordStage1Map()
///             : base("OVRHL_STAGE1_VERSION")
///         {
///             Map(x => x.Year, "YEAR").Not.Nullable();
///             Map(x => x.Sum, "SUM").Not.Nullable();
///             Map(x => x.SumService, "SERVICE_SUM").Not.Nullable();
///             Map(x => x.Volume, "VOLUME").Not.Nullable();
///             Map(x => x.TypeDpkrRecord, "TYPE_DPKR_RECORD").Not.Nullable().CustomType<TypeDpkrRecord>();
/// 
///             References(x => x.RealityObject, "REALITY_OBJECT_ID").Not.Nullable().Fetch.Join();
///             References(x => x.Stage2Version, "STAGE2_VERSION_ID").Not.Nullable().Fetch.Join();
///             References(x => x.StructuralElement, "STRUCT_EL_ID").Fetch.Join();
///             References(x => x.StrElement, "STR_EL_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Tat.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Tat.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Tat.Entities.VersionRecordStage1"</summary>
    public class VersionRecordStage1Map : BaseEntityMap<VersionRecordStage1>
    {
        
        public VersionRecordStage1Map() : 
                base("Bars.Gkh.Overhaul.Tat.Entities.VersionRecordStage1", "OVRHL_STAGE1_VERSION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Year, "Year").Column("YEAR").NotNull();
            Property(x => x.Sum, "Sum").Column("SUM").NotNull();
            Property(x => x.SumService, "SumService").Column("SERVICE_SUM").NotNull();
            Property(x => x.Volume, "Volume").Column("VOLUME").NotNull();
            Property(x => x.TypeDpkrRecord, "TypeDpkrRecord").Column("TYPE_DPKR_RECORD").NotNull();
            Reference(x => x.RealityObject, "RealityObject").Column("REALITY_OBJECT_ID").NotNull().Fetch();
            Reference(x => x.Stage2Version, "Stage2Version").Column("STAGE2_VERSION_ID").NotNull().Fetch();
            Reference(x => x.StructuralElement, "StructuralElement").Column("STRUCT_EL_ID").Fetch();
            Reference(x => x.StrElement, "StrElement").Column("STR_EL_ID").Fetch();
        }
    }
}
