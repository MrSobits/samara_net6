/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.GkhDi.Entities;
///     using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Работа по ТО"
///     /// </summary>
///     public class WorkRepairTechServMap : BaseGkhEntityMap<WorkRepairTechServ>
///     {
///         public WorkRepairTechServMap(): base("DI_REPAIR_WORK_TECH")
///         {
///             References(x => x.BaseService, "BASE_SERVICE_ID").Not.Nullable().Fetch.Join();
///             References(x => x.WorkTo, "WORK_TO_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.WorkRepairTechServ"</summary>
    public class WorkRepairTechServMap : BaseImportableEntityMap<WorkRepairTechServ>
    {
        
        public WorkRepairTechServMap() : 
                base("Bars.GkhDi.Entities.WorkRepairTechServ", "DI_REPAIR_WORK_TECH")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Reference(x => x.BaseService, "BaseService").Column("BASE_SERVICE_ID").NotNull().Fetch();
            Reference(x => x.WorkTo, "WorkTo").Column("WORK_TO_ID").Fetch();
        }
    }
}
