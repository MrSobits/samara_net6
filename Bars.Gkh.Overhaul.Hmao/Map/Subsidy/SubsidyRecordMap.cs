/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Hmao.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Hmao.Entities;
/// 
///     public class SubsidyRecordMap : BaseImportableEntityMap<SubsidyRecord>
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
///             Map(x => x.DateCalcOwnerCollection, "DATE_CALC_OWN");
/// 
///             References(x => x.Municiaplity, "MUNICIPALITY_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Hmao.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    
    
    /// <summary>Маппинг для "Запись субсидии В этой записи будут содержатся значения которые не зависят от версии (Но которые либо расчитываются либо забиваются в ручную)"</summary>
    public class SubsidyRecordMap : BaseImportableEntityMap<SubsidyRecord>
    {
        
        public SubsidyRecordMap() : 
                base("Запись субсидии В этой записи будут содержатся значения которые не зависят от вер" +
                        "сии (Но которые либо расчитываются либо забиваются в ручную)", "OVRHL_SUBSIDY_REC")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Municiaplity, "Муниципальное образование").Column("MUNICIPALITY_ID").Fetch();
            Property(x => x.SubsidyYear, "Год").Column("SUBCIDY_YEAR").NotNull();
            Property(x => x.BudgetRegion, "Бюджет региона").Column("BUDGET_REGION").NotNull();
            Property(x => x.BudgetMunicipality, "Бюджет МО").Column("BUDGET_MU").NotNull();
            Property(x => x.SaldoBallance, "Сальдо нарастающим итогом").Column("SALDO_BALLANCE").NotNull();
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
