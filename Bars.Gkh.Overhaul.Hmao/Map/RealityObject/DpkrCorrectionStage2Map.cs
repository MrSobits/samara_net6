/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Hmao.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Hmao.Entities;
/// 
///     public class DpkrCorrectionStage2Map : BaseImportableEntityMap<DpkrCorrectionStage2>
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

namespace Bars.Gkh.Overhaul.Hmao.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using System;
    
    
    /// <summary>Маппинг для "Сущность, содержащая данные, необходимые при учете корректировки ДПКР Лимит займа, Дефицит ..."</summary>
    public class DpkrCorrectionStage2Map : BaseImportableEntityMap<DpkrCorrectionStage2>
    {
        
        public DpkrCorrectionStage2Map() : 
                base("Сущность, содержащая данные, необходимые при учете корректировки ДПКР Лимит займа" +
                        ", Дефицит ...", "OVRHL_DPKR_CORRECT_ST2")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.RealityObject, "Объект недвижимости").Column("REALITYOBJECT_ID").NotNull().Fetch();
            Reference(x => x.Stage2, "Этап 2 формирования ДПКР").Column("ST2_VERSION_ID").NotNull().Fetch();
            Property(x => x.PlanYear, "Скорректированный год").Column("PLAN_YEAR").NotNull();
            Property(x => x.YearCollection, "Собираемость к году").Column("YEAR_COLLECTION").NotNull();
            Property(x => x.OwnersMoneyBalance, "Остаток средств собственников").Column("OWNERS_MONEY_BALANCE").NotNull();
            Property(x => x.HasCredit, "Есть непогашенный кредит").Column("HAS_CREDIT").DefaultValue(false).NotNull();
            Property(x => x.BudgetFundBalance, "Бюджет фонда").Column("BUDGET_BALANCE").NotNull();
            Property(x => x.BudgetRegionBalance, "Бюджет региона").Column("BUDGET_REGION").NotNull();
            Property(x => x.BudgetMunicipalityBalance, "Бюджет МО").Column("BUDGET_MUNICIPALITY").NotNull();
            Property(x => x.OtherSourceBalance, "Дриугие источники").Column("OTHER_BALANCE").NotNull();
            Property(x => x.IsOwnerMoneyBalanceCalculated, "Признак того, что строка поучавствовала в расчете баланса собственников и ее счит" +
                    "ать заново не надо").Column("IS_OWNER_CALCULATED").DefaultValue(false).NotNull();
        }
    }
}
