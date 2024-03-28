namespace Bars.Gkh.RegOperator.Map.Period
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities.Period;

    public class PeriodCloseRollbackHistoryMap : BaseEntityMap<PeriodCloseRollbackHistory>
    {
        public PeriodCloseRollbackHistoryMap()
            : base("История отката периода", "REGOP_PERIOD_ROLLBACK_HISTORY")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.PeriodName, "Наименование периода").Column("PERIOD_NAME").NotNull();
            this.Reference(x => x.User, "Изменивший пользователь").Column("USER_ID").NotNull();
            this.Reference(x => x.Period, "Период на который откатили").Column("PERIOD_ID").NotNull();
            this.Property(x => x.Date, "Дата отката").Column("DATE").NotNull();
            this.Property(x => x.Reason, "Причина").Column("REASON").NotNull();
        }
    }
}