namespace Bars.Gkh.RegOperator.Map.PersonalAccount
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;

    public class CalcDebtDetailMap : BaseEntityMap<CalcDebtDetail>
    {

        public CalcDebtDetailMap() :
            base("Детализация расчета долга", "REGOP_CALC_DEBT_DETAIL")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.CalcDebt, "Операция расчет долга").Column("CALC_DEBT_ID").NotNull().Fetch();
            this.Reference(x => x.AccountOwner, "Абонент").Column("OWNER_ID").NotNull().Fetch();
            this.Reference(x => x.PrevPersonalAccount, "Лицевой счет").Column("ACCOUNT_ID");
            this.Property(x => x.Type, "Тип").Column("TYPE").NotNull();
            this.Property(x => x.ChargeBaseTariff, "Начислено по БТ").Column("CHARGE_BASE_TARIFF");
            this.Property(x => x.ChargeDecTariff, "Начислено по ТР").Column("CHARGE_DEC_TARIFF");
            this.Property(x => x.ChargePenalty, "Начислено по пени").Column("CHARGE_PENALTY");
            this.Property(x => x.DistributionDebtBaseTariff, "Распределение долга по БТ").Column("DISTR_DEBT_BASE_TARIFF");
            this.Property(x => x.DistributionDebtDecTariff, "Распределение долга по ТР").Column("DISTR_DEBT_DEC_TARIFF");
            this.Property(x => x.DistributionDebtPenalty, "Распределение долга по пени").Column("DISTR_DEBT_PENALTY");
            this.Property(x => x.DistributionPayBaseTariff, "Распределение оплаты по БТ").Column("DISTR_PAY_BASE_TARIFF");
            this.Property(x => x.DistributionPayDecTariff, "Распределение оплаты по ТР").Column("DISTR_PAY_DEC_TARIFF");
            this.Property(x => x.DistributionPayPenalty, "Распределение оплаты по пени").Column("DISTR_PAY_PENALTY");
            this.Property(x => x.PaymentBaseTariff, "Оплата по БТ").Column("PAYMENT_BASE_TARIFF");
            this.Property(x => x.PaymentDecTariff, "Оплата по ТР").Column("PAYMENT_DEC_TARIFF");
            this.Property(x => x.PaymentPenalty, "Оплата по пени").Column("PAYMENT_PENALTY");
            this.Property(x => x.SaldoOutBaseTariff, "Исходящее сальдо по БТ").Column("SALDO_OUT_BASE_TARIFF");
            this.Property(x => x.SaldoOutDecisionTariff, "Исходящее сальдо по БТ").Column("SALDO_OUT_DEC_TARIFF");
            this.Property(x => x.SaldoOutPenalty, "Исходящее сальдо по пени").Column("SALDO_OUT_PENALTY");
            this.Property(x => x.IsImported, "Лицевой счет импортирован").Column("IS_IMPORTED");
        }
    }
}