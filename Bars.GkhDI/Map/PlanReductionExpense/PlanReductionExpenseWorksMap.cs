/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
/// 
///     using Entities;
///     using B4.DataAccess;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Работы по плану мер по снижению расходов"
///     /// </summary>
///     public class PlanReductionExpenseWorksMap : BaseGkhEntityMap<PlanReductionExpenseWorks>
///     {
///         public PlanReductionExpenseWorksMap()
///             : base("DI_DISINFO_RO_REDEXP_WORK")
///         {
///             Map(x => x.Name, "NAME").Length(300);
///             Map(x => x.DateComplete, "DATE_COMPLETE");
///             Map(x => x.PlannedReductionExpense, "PLAN_REDUCT_EXPENSE");
///             Map(x => x.FactedReductionExpense, "FACT_REDUCT_EXPENSE");
///             Map(x => x.ReasonRejection, "REASON_REJECTION").Length(500);
/// 
/// 
///             References(x => x.PlanReductionExpense, "DISINFO_RO_REDEXP_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.PlanReductionExpenseWorks"</summary>
    public class PlanReductionExpenseWorksMap : BaseImportableEntityMap<PlanReductionExpenseWorks>
    {
        
        public PlanReductionExpenseWorksMap() : 
                base("Bars.GkhDi.Entities.PlanReductionExpenseWorks", "DI_DISINFO_RO_REDEXP_WORK")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Name").Column("NAME").Length(300);
            Property(x => x.DateComplete, "DateComplete").Column("DATE_COMPLETE");
            Property(x => x.PlannedReductionExpense, "PlannedReductionExpense").Column("PLAN_REDUCT_EXPENSE");
            Property(x => x.FactedReductionExpense, "FactedReductionExpense").Column("FACT_REDUCT_EXPENSE");
            Property(x => x.ReasonRejection, "ReasonRejection").Column("REASON_REJECTION").Length(500);
            Reference(x => x.PlanReductionExpense, "PlanReductionExpense").Column("DISINFO_RO_REDEXP_ID").NotNull().Fetch();
        }
    }
}
