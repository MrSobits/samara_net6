namespace Bars.Gkh.RegOperator.Map.Period
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities.Period;

    public class PeriodCloseCheckHistoryMap : PersistentObjectMap<PeriodCloseCheckHistory>
    {
        public PeriodCloseCheckHistoryMap()
            : base("История изменения проверки периода", "REGOP_PERIOD_CLS_CHCK_HIST")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.Check, "Проверка").Column("CHECK_ID").NotNull();
            this.Reference(x => x.User, "Изменивший пользователь").Column("USER_ID").NotNull();

            this.Property(x => x.ChangeDate, "Дата изменения").Column("CHANGE_DATE").NotNull();
            this.Property(x => x.IsCritical, "Обязательность").Column("IS_CRITICAL").NotNull();
        }
    }
}