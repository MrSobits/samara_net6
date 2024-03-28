/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.GkhDi.Entities;
///     using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
/// 
///     /// <summary>
///     /// Маппинг для сущности "ППР детализация работ"
///     /// </summary>
///     public class WorkRepairDetailMap : BaseGkhEntityMap<WorkRepairDetail>
///     {
///         public WorkRepairDetailMap()
///             : base("DI_REPAIR_WORK_DETAIL")
///         {
///             References(x => x.BaseService, "BASE_SERVICE_ID").Not.Nullable().Fetch.Join();
///             References(x => x.WorkPpr, "WORK_PPR_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.WorkRepairDetail"</summary>
    public class WorkRepairDetailMap : BaseImportableEntityMap<WorkRepairDetail>
    {
        
        public WorkRepairDetailMap() : 
                base("Bars.GkhDi.Entities.WorkRepairDetail", "DI_REPAIR_WORK_DETAIL")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Reference(x => x.BaseService, "BaseService").Column("BASE_SERVICE_ID").NotNull().Fetch();
            Reference(x => x.WorkPpr, "WorkPpr").Column("WORK_PPR_ID").Fetch();
        }
    }
}
