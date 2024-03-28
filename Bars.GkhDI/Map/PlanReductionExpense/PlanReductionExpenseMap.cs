/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
/// 
///     using Entities;
///     using B4.DataAccess;
///     using Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "План мер по снижению расходов"
///     /// </summary>
///     public class PlanReductionExpenseMap : BaseGkhEntityMap<PlanReductionExpense>
///     {
///         public PlanReductionExpenseMap(): base("DI_DISINFO_RO_REDUCT_EXP")
///         {
/// 
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
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.PlanReductionExpense"</summary>
    public class PlanReductionExpenseMap : BaseImportableEntityMap<PlanReductionExpense>
    {
        
        public PlanReductionExpenseMap() : 
                base("Bars.GkhDi.Entities.PlanReductionExpense", "DI_DISINFO_RO_REDUCT_EXP")
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
