/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Regions.Tatarstan.Map
/// {
///     using Bars.GkhDi.Regions.Tatarstan.Entities;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Маппинг для сущности "ППР список работ" (Для Татарстана)
///     /// </summary>
///     public class WorkRepairListTatMap : SubclassMap<WorkRepairListTat>
///     {
///         public WorkRepairListTatMap()
///         {
///             this.Table("DI_REPAIR_WRK_LIST_TAT");
///             this.KeyColumn("ID");
/// 
///             this.Map(x => x.ReasonRejection, "REASON_REJECTION").Length(500);
///             this.Map(x => x.InfoAboutExec, "INFO_EXEC").Length(500);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Regions.Tatarstan.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Regions.Tatarstan.Entities.WorkRepairListTat"</summary>
    public class WorkRepairListTatMap : JoinedSubClassMap<WorkRepairListTat>
    {
        
        public WorkRepairListTatMap() : 
                base("Bars.GkhDi.Regions.Tatarstan.Entities.WorkRepairListTat", "DI_REPAIR_WRK_LIST_TAT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ReasonRejection, "ReasonRejection").Column("REASON_REJECTION").Length(500);
            Property(x => x.InfoAboutExec, "InfoAboutExec").Column("INFO_EXEC").Length(500);
        }
    }
}
