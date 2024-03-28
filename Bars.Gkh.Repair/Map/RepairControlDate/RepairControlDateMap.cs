/// <mapping-converter-backup>
/// namespace Bars.Gkh.Repair.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.Gkh.Repair.Entities.RepairControlDate;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Контрольные сроки работ по текущему ремонту"
///     /// </summary>
/// 
///     public class RepairControlDateMap : BaseEntityMap<RepairControlDate>
///     {
///         public RepairControlDateMap()
///             : base("RP_CONTROL_DATE")
///         {
///             Map(x => x.Date, "RP_DATE_CTRL");
/// 
///             References(x => x.RepairProgram, "RP_PROGRAM_ID").Fetch.Join().Not.Nullable();
///             References(x => x.Work, "RP_WORK_ID").Fetch.Join().Not.Nullable();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Repair.Map.RepairControlDate
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Repair.Entities.RepairControlDate;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Repair.Entities.RepairControlDate.RepairControlDate"</summary>
    public class RepairControlDateMap : BaseEntityMap<RepairControlDate>
    {
        
        public RepairControlDateMap() : 
                base("Bars.Gkh.Repair.Entities.RepairControlDate.RepairControlDate", "RP_CONTROL_DATE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Date, "Date").Column("RP_DATE_CTRL");
            Reference(x => x.RepairProgram, "RepairProgram").Column("RP_PROGRAM_ID").NotNull().Fetch();
            Reference(x => x.Work, "Work").Column("RP_WORK_ID").NotNull().Fetch();
        }
    }
}
