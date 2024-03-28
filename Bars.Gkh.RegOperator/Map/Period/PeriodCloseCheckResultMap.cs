namespace Bars.Gkh.RegOperator.Map.Period
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities.Period;
    using Bars.Gkh.RegOperator.Enums;

    public class PeriodCloseCheckResultMap : PersistentObjectMap<PeriodCloseCheckResult>
    {
        public PeriodCloseCheckResultMap()
            : base("Результат проверки периода перед закрытием", "REGOP_PERIOD_CLS_CHCK_RES")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.CheckDate, "Дата время проверки").Column("CHECK_DATE").NotNull();
            this.Property(x => x.CheckState, "Состояние проверки").Column("CHECK_STATE").NotNull().DefaultValue((int)PeriodCloseCheckStateType.Pending);
            this.Property(x => x.Impl, "Системный код проверки").Column("IMPL").NotNull().Length(255);
            this.Property(x => x.Code, "Код").Column("CODE").NotNull().Length(255);
            this.Property(x => x.Name, "Отображаемое имя проверки").Column("NAME").NotNull().Length(500);
            this.Property(x => x.IsCritical, "Обязательность").Column("IS_CRITICAL").NotNull();
            this.Property(x => x.Note, "Сообщение").Column("NOTE").Length(1000);

            this.Reference(x => x.LogFile, "Лог проверки").Column("LOG_FILE_ID");
            this.Reference(x => x.FullLogFile, "Полный Лог проверки для каждой записи").Column("FULL_LOG_FILE_ID");
            this.Reference(x => x.Period, "Проверяемый период").Column("PERIOD_ID").NotNull();
            this.Reference(x => x.User, "Пользователь, инициировавший проверку").Column("USER_ID").NotNull();
            this.Reference(x => x.PersAccGroup, "Группа ЛС, в которую скидываются ЛС не прошедшие проверку").Column("GROUP_ID");
        }
    }
}