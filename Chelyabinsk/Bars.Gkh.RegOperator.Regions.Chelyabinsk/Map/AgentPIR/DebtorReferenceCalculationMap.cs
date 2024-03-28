namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    public class DebtorReferenceCalculationMap : BaseEntityMap<DebtorReferenceCalculation>
    {
        public DebtorReferenceCalculationMap() :  base("Эталонный расчет даты начала задолженности", "AGENT_PIR_DEBTOR_REFERENCE_CALCULATION") {}

        protected override void Map()
        {
            this.Property(x => x.AccountNumber, "Номер ЛС").Column("ACC_NUM");
            this.Property(x => x.AreaShare, "Доля собственности").Column("AREA_SHARE").NotNull();
            this.Property(x => x.BaseTariff, "Тариф").Column("BASE_TARIF").NotNull();
            this.Property(x => x.PeriodId, "Период").Column("PERIOD_ID").NotNull();
            this.Property(x => x.PersonalAccountId, "Лицевой счет").Column("ACCOUNT_ID").NotNull();
            this.Property(x => x.RoomArea, "Площадь помещения").Column("ROOM_AREA").NotNull();
            this.Property(x => x.TarifDebt, "Задолженность").Column("TARIF_DEBT");
            this.Property(x => x.TarifDebtPay, "Задолженность с учетом погашений").Column("TARIF_DEBT_PAY");
            this.Reference(x => x.AgentPIRDebtor, "AgentPIRDebtor").Column("DEBTOR_ID").NotNull();
            this.Property(x => x.TariffCharged, "Начислено").Column("TARIF_CHARGED");
            this.Property(x => x.PaymentDate, "Дата оплаты").Column("PAYMENT_DATE");
            this.Property(x => x.TarifPayment, "Уплачено").Column("TARIF_PAYMENTS");
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION");
            this.Property(x => x.Penalties, "Пени").Column("PENALTIES");
            this.Property(x => x.PenaltyPayment, "Пени").Column("PENALTY_PAYMENT");
            this.Property(x => x.PenaltyPaymentDate, "Пени").Column("PENALTY_PAYMENT_DATE");
            this.Property(x => x.AccrualPenalties, "Начисление пени").Column("ACCRUAL_PENALTIES");
            this.Property(x => x.AccrualPenaltiesFormula, "Начисление пени").Column("ACCRUAL_PENALTIES_FORM");
        }
    }
}
