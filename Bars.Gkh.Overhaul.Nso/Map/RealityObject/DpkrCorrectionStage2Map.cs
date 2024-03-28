/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Nso.Map.RealityObject
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Nso.Entities;
/// 
///     public class DpkrCorrectionStage2Map : BaseEntityMap<DpkrCorrectionStage2>
///     {
///         public DpkrCorrectionStage2Map()
///             : base("OVHL_DPKR_CORRECT_ST2")
///         {
///             Map(x => x.OwnersMoneyBalance, "OWNERS_MONEY_BALANCE", true, 0);
///             Map(x => x.PlanYear, "PLAN_YEAR", true, 0);
///             Map(x => x.YearCollection, "YEAR_COLLECTION", true, 0);
///             Map(x => x.HasCredit, "HAS_CREDIT", true, false);
///             Map(x => x.BudgetFundBalance, "BUDGET_BALANCE", true, 0);
///             Map(x => x.BudgetRegionBalance, "BUDGET_REGION", true, 0);
///             Map(x => x.BudgetMunicipalityBalance, "BUDGET_MUNICIPALITY", true, 0);
///             Map(x => x.OtherSourceBalance, "OTHER_BALANCE", true, 0);
///             Map(x => x.IsOwnerMoneyBalanceCalculated, "IS_OWNER_CALCULATED", true, false);
/// 
///             References(x => x.RealityObject, "REALITYOBJECT_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.Stage2, "ST2_VERSION_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Nso.Entities;
    using System;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Nso.Entities.DpkrCorrectionStage2"</summary>
    public class DpkrCorrectionStage2Map : BaseEntityMap<DpkrCorrectionStage2>
    {
        
        public DpkrCorrectionStage2Map() : 
                base("Bars.Gkh.Overhaul.Nso.Entities.DpkrCorrectionStage2", "OVHL_DPKR_CORRECT_ST2")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.RealityObject, "RealityObject").Column("REALITYOBJECT_ID").NotNull().Fetch();
            Property(x => x.PlanYear, "PlanYear").Column("PLAN_YEAR").NotNull();
            Property(x => x.YearCollection, "YearCollection").Column("YEAR_COLLECTION").NotNull();
            Property(x => x.OwnersMoneyBalance, "OwnersMoneyBalance").Column("OWNERS_MONEY_BALANCE").NotNull();
            Property(x => x.HasCredit, "HasCredit").Column("HAS_CREDIT").DefaultValue(false).NotNull();
            Property(x => x.BudgetFundBalance, "BudgetFundBalance").Column("BUDGET_BALANCE").NotNull();
            Property(x => x.BudgetRegionBalance, "BudgetRegionBalance").Column("BUDGET_REGION").NotNull();
            Property(x => x.BudgetMunicipalityBalance, "BudgetMunicipalityBalance").Column("BUDGET_MUNICIPALITY").NotNull();
            Property(x => x.OtherSourceBalance, "OtherSourceBalance").Column("OTHER_BALANCE").NotNull();
            Reference(x => x.Stage2, "Stage2").Column("ST2_VERSION_ID").NotNull().Fetch();
            Property(x => x.IsOwnerMoneyBalanceCalculated, "IsOwnerMoneyBalanceCalculated").Column("IS_OWNER_CALCULATED").DefaultValue(false).NotNull();
        }
    }
}
