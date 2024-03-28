namespace Bars.Gkh.RegOperator.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities.Dict;

    /// <summary>
    /// Миграция для Настройки расчета пени с отсрочкой
    /// </summary>
    public class FixedPeriodCalcPenaltiesMap : BaseImportableEntityMap<FixedPeriodCalcPenalties>
    {
        public FixedPeriodCalcPenaltiesMap() :
                base("Настройки расчета пени с отсрочкой", "REGOP_FIX_PER_CALC_PENALTIES")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.StartDay, "День начала периода").Column("START_DAY").NotNull();
            this.Property(x => x.EndDay, "День окончания периода").Column("END_DAY").NotNull();
            this.Property(x => x.DateStart, "Действует с").Column("DATE_START").NotNull();
            this.Property(x => x.DateEnd, "Действует по").Column("DATE_END");
        }
    }
}