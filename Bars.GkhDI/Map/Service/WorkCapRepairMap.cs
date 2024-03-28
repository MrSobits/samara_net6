/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.GkhDi.Entities;
///     using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Работа услуги капремонт"
///     /// </summary>
///     public class WorkCapRepairMap : BaseGkhEntityMap<WorkCapRepair>
///     {
///         public WorkCapRepairMap()
///             : base("DI_CAPREPAIR_WORK")
///         {
///             Map(x => x.PlannedVolume, "PLAN_VOLUME");
///             Map(x => x.PlannedCost, "PLAN_COST");
///             Map(x => x.FactedVolume, "FACT_VOLUME");
///             Map(x => x.FactedCost, "FACT_COST");
/// 
///             References(x => x.BaseService, "BASE_SERVICE_ID").Not.Nullable().Fetch.Join();
///             References(x => x.Work, "WORK_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.WorkCapRepair"</summary>
    public class WorkCapRepairMap : BaseImportableEntityMap<WorkCapRepair>
    {
        
        public WorkCapRepairMap() : 
                base("Bars.GkhDi.Entities.WorkCapRepair", "DI_CAPREPAIR_WORK")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.PlannedVolume, "PlannedVolume").Column("PLAN_VOLUME");
            Property(x => x.PlannedCost, "PlannedCost").Column("PLAN_COST");
            Property(x => x.FactedVolume, "FactedVolume").Column("FACT_VOLUME");
            Property(x => x.FactedCost, "FactedCost").Column("FACT_COST");
            Reference(x => x.BaseService, "BaseService").Column("BASE_SERVICE_ID").NotNull().Fetch();
            Reference(x => x.Work, "Work").Column("WORK_ID").Fetch();
        }
    }
}
