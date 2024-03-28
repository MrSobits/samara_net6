/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Nso.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Nso.Entities;
/// 
///     public class SubsidyRecordMap : BaseEntityMap<SubsidyRecord>
///     {
///         public SubsidyRecordMap()
///             : base("OVRHL_SUBSIDY_REC")
///         {
///             Map(x => x.SubsidyYear, "SUBCIDY_YEAR", true, 0);
///             Map(x => x.BudgetRegion, "BUDGET_REGION", true, 0);
///             Map(x => x.BudgetMunicipality, "BUDGET_MU", true, 0);
///             Map(x => x.BudgetFcr, "BUDGET_FSR", true, 0);
///             Map(x => x.BudgetOtherSource, "BUDGET_OTHER_SRC", true, 0);
///             Map(x => x.PlanOwnerCollection, "PLAN_OWN_COLLECTION", true, 0);
///             Map(x => x.PlanOwnerPercent, "PLAN_OWN_PRC", true, 0);
///             Map(x => x.NotReduceSizePercent, "NOT_REDUCE_SIZE_PRC", true, 0);
///             Map(x => x.OwnerSumForCr, "OWNER_SUM_CR", true, 0);
///             Map(x => x.Reserve, "RESERVE", true, 0);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Nso.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Nso.Entities.SubsidyRecord"</summary>
    public class SubsidyRecordMap : BaseEntityMap<SubsidyRecord>
    {
        
        public SubsidyRecordMap() : 
                base("Bars.Gkh.Overhaul.Nso.Entities.SubsidyRecord", "OVRHL_SUBSIDY_REC")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.SubsidyYear, "SubsidyYear").Column("SUBCIDY_YEAR").NotNull();
            Property(x => x.BudgetRegion, "BudgetRegion").Column("BUDGET_REGION").NotNull();
            Property(x => x.BudgetMunicipality, "BudgetMunicipality").Column("BUDGET_MU").NotNull();
            Property(x => x.BudgetFcr, "BudgetFcr").Column("BUDGET_FSR").NotNull();
            Property(x => x.BudgetOtherSource, "BudgetOtherSource").Column("BUDGET_OTHER_SRC").NotNull();
            Property(x => x.PlanOwnerCollection, "PlanOwnerCollection").Column("PLAN_OWN_COLLECTION").NotNull();
            Property(x => x.PlanOwnerPercent, "PlanOwnerPercent").Column("PLAN_OWN_PRC").NotNull();
            Property(x => x.NotReduceSizePercent, "NotReduceSizePercent").Column("NOT_REDUCE_SIZE_PRC").NotNull();
            Property(x => x.OwnerSumForCr, "OwnerSumForCr").Column("OWNER_SUM_CR").NotNull();
            Property(x => x.Reserve, "Reserve").Column("RESERVE").NotNull();
        }
    }
}
