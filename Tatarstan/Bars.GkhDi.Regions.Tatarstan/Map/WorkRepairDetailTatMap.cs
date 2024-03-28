/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Regions.Tatarstan.Map
/// {
///     using Bars.GkhDi.Regions.Tatarstan.Entities;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Маппинг для сущности "ППР детализация работ" (Для Татарстана)
///     /// </summary>
///     public class WorkRepairDetailTatMap : SubclassMap<WorkRepairDetailTat>
///     {
///         public WorkRepairDetailTatMap()
///         {
///             this.Table("DI_REPAIR_WRK_DET_TAT");
///             this.KeyColumn("ID");
/// 
///             this.Map(x => x.FactVolume, "FACT_VOLUME");
///             this.Map(x => x.PlannedVolume, "PLAN_VOLUME");
/// 
///             this.References(x => x.UnitMeasure, "MEASURE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Regions.Tatarstan.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Regions.Tatarstan.Entities.WorkRepairDetailTat"</summary>
    public class WorkRepairDetailTatMap : JoinedSubClassMap<WorkRepairDetailTat>
    {
        
        public WorkRepairDetailTatMap() : 
                base("Bars.GkhDi.Regions.Tatarstan.Entities.WorkRepairDetailTat", "DI_REPAIR_WRK_DET_TAT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.FactVolume, "FactVolume").Column("FACT_VOLUME");
            Property(x => x.PlannedVolume, "PlannedVolume").Column("PLAN_VOLUME");
            Reference(x => x.UnitMeasure, "UnitMeasure").Column("MEASURE_ID").Fetch();
        }
    }
}
