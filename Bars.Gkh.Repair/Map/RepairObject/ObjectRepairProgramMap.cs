/// <mapping-converter-backup>
/// namespace Bars.Gkh.Repair.Map
/// {
///     using Bars.B4.DataAccess;
/// 
///     using Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Объект текущего ремонта"
///     /// </summary>
///     public class RepairObjectMap : BaseEntityMap<RepairObject>
///     {
///         public RepairObjectMap() : base("RP_OBJECT")
///         {
///             References(x => x.RepairProgram, "PROGRAM_ID").Not.Nullable().Fetch.Join();
///             References(x => x.RealityObject, "REALITY_OBJECT_ID").Not.Nullable().Fetch.Join();
///             References(x => x.State, "STATE_ID").Fetch.Join();
///         }
///     }
/// }
/// 
/// </mapping-converter-backup>

namespace Bars.Gkh.Repair.Map
{
    using B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Repair.Entities;
    
    /// <summary>Маппинг для "Bars.Gkh.Repair.Entities.RepairObject"</summary>
    public class RepairObjectMap : BaseEntityMap<Entities.RepairObject>
    {
        
        public RepairObjectMap() : 
                base("Bars.Gkh.Repair.Entities.RepairObject", "RP_OBJECT")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.RepairProgram, "RepairProgram").Column("PROGRAM_ID").NotNull().Fetch();
            this.Reference(x => x.RealityObject, "RealityObject").Column("REALITY_OBJECT_ID").NotNull().Fetch();
            this.Reference(x => x.State, "State").Column("STATE_ID").Fetch();
            this.Reference(x => x.ReasonDocument, "ReasonDocument").Column("REASON_DOCUMENT_ID").Fetch();
            this.Property(x => x.Comment, "Comment").Column("COMMENT").Length(2000);
        }
    }
}
