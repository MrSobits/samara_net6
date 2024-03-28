namespace Bars.Gkh.Overhaul.Hmao.Map
{
    using Bars.B4.Modules.Mapping.Mappers; 
    using Bars.Gkh.Overhaul.Hmao.Entities;


    /// <summary>
    /// Маппинг для "Контрагент"
    /// </summary>
    public class EconFeasibilityCalcResultMap : BaseEntityMap<EconFeasibilityCalcResult>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public EconFeasibilityCalcResultMap() :
                base("Результат расчета экономической целесообразности", "OVRHL_ECON_FEASIBILITY_CALC_RESULT")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.RoId, "Дом").Column("RO_ID").NotNull().Fetch();
            this.Reference(x => x.SquareCost, "Средняя стоимость квадратного метра").Column("SQUARE_COST_ID").NotNull().Fetch();           
            this.Property(x => x.YearStart, "Год начала").Column("YEAR_START");
            this.Property(x => x.YearEnd, "Год окончания").Column("YEAR_END");
            this.Property(x => x.TotatRepairSumm, "Полная стоимость ремонта дома").Column("TOTAL_REPAIR_SUMM");
            this.Property(x => x.TotalSquareCost, "Стоимость жилых и нежилых помещений").Column("TOTAL_SQUARE_COST");
            this.Property(x => x.CostPercent, "Процент стоимости ремонта от стоимости помещений").Column("PERCENT");
            this.Property(x => x.Decision, "Решение").Column("DECISION").DefaultValue(0);
        }
    }
}
