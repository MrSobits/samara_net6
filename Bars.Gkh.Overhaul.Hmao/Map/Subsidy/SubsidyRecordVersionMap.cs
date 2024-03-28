/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Hmao.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Hmao.Entities;
/// 
///     public class SubsidyRecordVersionMap : BaseImportableEntityMap<SubsidyRecordVersion>
///     {
///         public SubsidyRecordVersionMap()
///             : base("OVRHL_SUBSIDY_REC_VERSION")
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
///             Map(x => x.DateCalcOwnerCollection, "DATE_CALC_OWN");
/// 
///             Map(x => x.BudgetCr, "BUDGET_CR", true, 0);
///             Map(x => x.CorrectionFinance, "CORRECTION_FINANCES", true, 0);
///             Map(x => x.BalanceAfterCr, "BALANCE_AFTER_CR", true, 0);
/// 
///             References(x => x.SubsidyRecord, "SUBSIDY_REC_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.Version, "VERSION_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Hmao.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Hmao.Entities.SubsidyRecordVersion"</summary>
    public class SubsidyRecordVersionMap : BaseImportableEntityMap<SubsidyRecordVersion>
    {
        
        public SubsidyRecordVersionMap() : 
                base("Bars.Gkh.Overhaul.Hmao.Entities.SubsidyRecordVersion", "OVRHL_SUBSIDY_REC_VERSION")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Version, "Версия ДПКР").Column("VERSION_ID").NotNull().Fetch();
            Reference(x => x.SubsidyRecord, "Ссылка на строку субсидирования").Column("SUBSIDY_REC_ID").NotNull().Fetch();
            Property(x => x.BudgetCr, "Бюджет на КР (Привязываю к версии посколку все это расчитывается на суммы именно " +
                    "той версии)").Column("BUDGET_CR").NotNull();
            Property(x => x.CorrectionFinance, "Потребность в финансировании").Column("CORRECTION_FINANCES").NotNull();
            Property(x => x.BalanceAfterCr, "Остаток средств после проведения КР на конец года").Column("BALANCE_AFTER_CR").NotNull();
            Property(x => x.AdditionalExpences, "Остаток средств после проведения КР на конец года").Column("ADDIT_EXPENCES").NotNull();
            Property(x => x.SubsidyYear, "Год").Column("SUBCIDY_YEAR").NotNull();
            Property(x => x.BudgetRegion, "Бюджет региона").Column("BUDGET_REGION").NotNull();
            Property(x => x.SaldoBallance, "Сальдо нарастающим итогом").Column("SALDO_BALLANCE").NotNull();
            Property(x => x.BudgetMunicipality, "Бюджет МО").Column("BUDGET_MU").NotNull();
            Property(x => x.BudgetFcr, "Бюджет ФСР").Column("BUDGET_FSR").NotNull();
            Property(x => x.BudgetOtherSource, "Бюджет других источников").Column("BUDGET_OTHER_SRC").NotNull();
            Property(x => x.PlanOwnerCollection, "Плановая собираемость (То есть сколько средств по плану соберут собственники)").Column("PLAN_OWN_COLLECTION").NotNull();
            Property(x => x.PlanOwnerPercent, "(% Забивается в ручную) Процент собираемости (То есть ест ьвероятность того что н" +
                    "есоберут нужную сумма) и поэтому заводят процент который покажет Нормальную сумм" +
                    "у средств собственников").Column("PLAN_OWN_PRC").NotNull();
            Property(x => x.NotReduceSizePercent, "(% Забивается в ручную) Не снижаемый размер регионального фонда").Column("NOT_REDUCE_SIZE_PRC").NotNull();
            Property(x => x.OwnerSumForCr, "Средства собственников на кап. ремонт").Column("OWNER_SUM_CR").NotNull();
            Property(x => x.DateCalcOwnerCollection, "Дата расчета собираемости").Column("DATE_CALC_OWN");
        }
    }
}
