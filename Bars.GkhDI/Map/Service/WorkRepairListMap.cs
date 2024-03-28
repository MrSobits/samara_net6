/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.GkhDi.Entities;
///     using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
/// 
///     /// <summary>
///     /// Маппинг для сущности "ППР список работ"
///     /// </summary>
///     public class WorkRepairListMap : BaseGkhEntityMap<WorkRepairList>
///     {
///         public WorkRepairListMap()
///             : base("DI_REPAIR_WORK_LIST")
///         {
///             Map(x => x.PlannedVolume, "PLAN_VOLUME");
///             Map(x => x.PlannedCost, "PLAN_COST");
///             Map(x => x.FactVolume, "FACT_VOLUME");
///             Map(x => x.FactCost, "FACT_COST");
///             Map(x => x.DateStart, "DATE_START");
///             Map(x => x.DateEnd, "DATE_END");
/// 
///             References(x => x.BaseService, "BASE_SERVICE_ID").Not.Nullable().Fetch.Join();
///             References(x => x.GroupWorkPpr, "GROUP_WORK_PPR_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.WorkRepairList"</summary>
    public class WorkRepairListMap : BaseImportableEntityMap<WorkRepairList>
    {
        
        public WorkRepairListMap() : 
                base("Bars.GkhDi.Entities.WorkRepairList", "DI_REPAIR_WORK_LIST")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.PlannedVolume, "PlannedVolume").Column("PLAN_VOLUME");
            Property(x => x.PlannedCost, "PlannedCost").Column("PLAN_COST");
            Property(x => x.FactVolume, "FactVolume").Column("FACT_VOLUME");
            Property(x => x.FactCost, "FactCost").Column("FACT_COST");
            Property(x => x.DateStart, "DateStart").Column("DATE_START");
            Property(x => x.DateEnd, "DateEnd").Column("DATE_END");
            Reference(x => x.BaseService, "BaseService").Column("BASE_SERVICE_ID").NotNull().Fetch();
            Reference(x => x.GroupWorkPpr, "GroupWorkPpr").Column("GROUP_WORK_PPR_ID").Fetch();
        }
    }
}
