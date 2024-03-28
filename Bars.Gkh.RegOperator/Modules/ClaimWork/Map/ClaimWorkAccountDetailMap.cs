namespace Bars.Gkh.RegOperator.Map
{
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;

    public class ClaimWorkAccountDetailMap : GkhBaseEntityMap<ClaimWorkAccountDetail>
    {
        public ClaimWorkAccountDetailMap() : base("CLW_CLAIM_WORK_ACC_DETAIL")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.PersonalAccount, "Лицевой счет").Column("ACCOUNT_ID").NotNull().Fetch();
            this.Reference(x => x.ClaimWork, "Основание ПИР").Column("CLAIM_WORK_ID").NotNull().Fetch();
            this.Property(x => x.CurrChargeBaseTariffDebt, "Сумма текущей задолженности по базовому тарифу").Column("CUR_CHARGE_BASE_TARIFF_DEBT").NotNull();
            this.Property(x => x.CurrChargeDecisionTariffDebt, "Сумма текущей задолженности по тарифу решения").Column("CUR_CHARGE_DECISION_TARIFF_DEBT").NotNull();
            this.Property(x => x.CurrChargeDebt, "Сумма текущей задолженности").Column("CUR_CHARGE_DEBT").NotNull();
            this.Property(x => x.CurrPenaltyDebt, "Сумма текущей задолженности по пени").Column("CUR_PENALTY_DEBT").NotNull();
            this.Property(x => x.OrigChargeBaseTariffDebt, "Сумма текущей задолженности по базовому тарифу").Column("ORIG_CHARGE_BASE_TARIFF_DEBT").NotNull();
            this.Property(x => x.OrigChargeDecisionTariffDebt, "Сумма текущей задолженности по тарифу решения").Column("ORIG_CHARGE_DECISION_TARIFF_DEBT").NotNull();
            this.Property(x => x.OrigChargeDebt, "Сумма исходной задолженности").Column("ORIG_CHARGE_DEBT").NotNull();
            this.Property(x => x.OrigPenaltyDebt, "Сумма исходной задолженности по пени").Column("ORIG_PENALTY_DEBT").NotNull();
            this.Property(x => x.StartingDate, "Дата начала отсчета").Column("START_DATE").NotNull();
            this.Property(x => x.CountDaysDelay, "Количество дней просрочки").Column("COUNT_DAYS_DELAY").NotNull();
            this.Property(x => x.CountMonthDelay, "Число месяцев просрочки").Column("COUNT_MONTH_DELAY").NotNull();
        }
    }
}