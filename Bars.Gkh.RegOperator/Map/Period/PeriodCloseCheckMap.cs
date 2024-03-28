namespace Bars.Gkh.RegOperator.Map.Period
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities.Period;

    public class PeriodCloseCheckMap : BaseEntityMap<PeriodCloseCheck>
    {
        public PeriodCloseCheckMap()
            : base("Проверка периода перед закрытием", "REGOP_PERIOD_CLS_CHCK")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Code, "Код").Column("CODE").NotNull().Length(255);
            this.Property(x => x.Impl, "Системный код проверки").Column("IMPL").NotNull().Length(255);
            this.Property(x => x.Name, "Отображаемое имя проверки").Column("NAME").NotNull().Length(500);
            this.Property(x => x.IsCritical, "Обязательность").Column("IS_CRITICAL").NotNull().DefaultValue(false);
        }
    }
}