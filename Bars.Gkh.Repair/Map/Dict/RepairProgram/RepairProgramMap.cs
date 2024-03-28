/// <mapping-converter-backup>
/// namespace Bars.Gkh.Repair.Map
/// {
///     using Bars.B4.DataAccess;
/// 
///     using Entities;
///     using Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Программа текущего ремонта"
///     /// </summary>
///     public class RepairProgramMap : BaseEntityMap<RepairProgram>
///     {
///         public RepairProgramMap() : base("RP_DICT_PROGRAM")
///         {
///             Map(x => x.TypeVisibilityProgramRepair, "TYPE_VISIBILITY").Not.Nullable().CustomType<TypeVisibilityProgramRepair>();
///             Map(x => x.TypeProgramRepairState, "TYPE_PROGRAM_STATE").Not.Nullable().CustomType<TypeProgramRepairState>();
///             Map(x => x.Name, "NAME").Length(300);
/// 
///             References(x => x.Period, "PERIOD_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// 
/// </mapping-converter-backup>

namespace Bars.Gkh.Repair.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Repair.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Repair.Entities.RepairProgram"</summary>
    public class RepairProgramMap : BaseEntityMap<RepairProgram>
    {
        
        public RepairProgramMap() : 
                base("Bars.Gkh.Repair.Entities.RepairProgram", "RP_DICT_PROGRAM")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.TypeVisibilityProgramRepair, "TypeVisibilityProgramRepair").Column("TYPE_VISIBILITY").NotNull();
            Property(x => x.TypeProgramRepairState, "TypeProgramRepairState").Column("TYPE_PROGRAM_STATE").NotNull();
            Property(x => x.Name, "Name").Column("NAME").Length(300);
            Reference(x => x.Period, "Period").Column("PERIOD_ID").NotNull().Fetch();
        }
    }
}
