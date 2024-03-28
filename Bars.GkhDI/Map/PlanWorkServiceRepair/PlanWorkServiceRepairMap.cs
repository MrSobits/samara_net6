/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
///     using Bars.GkhDi.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "План работ по содержанию и ремонту"
///     /// </summary>
///     public class PlanWorkServiceRepairMap : BaseGkhEntityMap<PlanWorkServiceRepair>
///     {
///         public PlanWorkServiceRepairMap() : base("DI_DISINFO_RO_SERV_REPAIR")
///         {
///             References(x => x.DisclosureInfoRealityObj, "DISINFO_RO_ID").Not.Nullable().Fetch.Join();
///             References(x => x.BaseService, "BASE_SERVICE_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.PlanWorkServiceRepair"</summary>
    public class PlanWorkServiceRepairMap : BaseImportableEntityMap<PlanWorkServiceRepair>
    {
        
        public PlanWorkServiceRepairMap() : 
                base("Bars.GkhDi.Entities.PlanWorkServiceRepair", "DI_DISINFO_RO_SERV_REPAIR")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Reference(x => x.DisclosureInfoRealityObj, "DisclosureInfoRealityObj").Column("DISINFO_RO_ID").NotNull().Fetch();
            Reference(x => x.BaseService, "BaseService").Column("BASE_SERVICE_ID").NotNull().Fetch();
        }
    }
}
