/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Tat.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.Gkh.Overhaul.Tat.Enum;
///     using Entities;
/// 
///     public class ShortProgramRecordMap : BaseEntityMap<ShortProgramRecord>
///     {
///         public ShortProgramRecordMap()
///             : base("OVRHL_SHORT_PROG_REC")
///         {
///             Map(x => x.Volume, "VOLUME").Not.Nullable();
///             Map(x => x.Cost, "COST").Not.Nullable();
///             Map(x => x.TotalCost, "TOTAL_COST").Not.Nullable();
///             Map(x => x.ServiceCost, "SERVICE_COST").Not.Nullable();
///             Map(x => x.TypeDpkrRecord, "TYPE_DPKR_RECORD").Not.Nullable().CustomType<TypeDpkrRecord>();
/// 
///             References(x => x.ShortProgramObject, "SHORT_PROG_OBJ_ID").Not.Nullable().Fetch.Join();
///             References(x => x.Work, "WORK_ID").Not.Nullable().Fetch.Join();
///             References(x => x.Stage1, "STAGE1_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Tat.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Tat.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Tat.Entities.ShortProgramRecord"</summary>
    public class ShortProgramRecordMap : BaseEntityMap<ShortProgramRecord>
    {
        
        public ShortProgramRecordMap() : 
                base("Bars.Gkh.Overhaul.Tat.Entities.ShortProgramRecord", "OVRHL_SHORT_PROG_REC")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Volume, "Volume").Column("VOLUME").NotNull();
            Property(x => x.Cost, "Cost").Column("COST").NotNull();
            Property(x => x.TotalCost, "TotalCost").Column("TOTAL_COST").NotNull();
            Property(x => x.ServiceCost, "ServiceCost").Column("SERVICE_COST").NotNull();
            Property(x => x.TypeDpkrRecord, "TypeDpkrRecord").Column("TYPE_DPKR_RECORD").NotNull();
            Reference(x => x.ShortProgramObject, "ShortProgramObject").Column("SHORT_PROG_OBJ_ID").NotNull().Fetch();
            Reference(x => x.Work, "Work").Column("WORK_ID").NotNull().Fetch();
            Reference(x => x.Stage1, "Stage1").Column("STAGE1_ID").Fetch();
        }
    }
}
