namespace Bars.Gkh.RegOperator.Map
{
    using Bars.Gkh.RegOperator.Entities;

    using Bars.Gkh.Map;

    /// <summary>Маппинг для "Начисления по счету начисления дома (группировка по периодам)"</summary>
    public class RealityObjectChargeAccountOperationMap : BaseImportableEntityMap<RealityObjectChargeAccountOperation>
    {
        
        public RealityObjectChargeAccountOperationMap() : 
                base("Начисления по счету начисления дома (группировка по периодам)", "REGOP_RO_CHARGE_ACC_CHARGE")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.Date, "Дата операции").Column("CDATE").NotNull();
            this.Reference(x => x.Period, "Период начислений").Column("PERIOD_ID");
            this.Reference(x => x.Account, "Счет начислений дома").Column("ACC_ID");
            this.Property(x => x.ChargedTotal, "Сумма по всем начисленям ЛС по дому за период").Column("CCHARGED").NotNull();
            this.Property(x => x.SaldoIn, "Входящее сальдо").Column("CSALDO_IN").NotNull();
            this.Property(x => x.SaldoOut, "Исходящее сальдо").Column("CSALDO_OUT").NotNull();
            this.Property(x => x.PaidTotal, "Оплачено всего").Column("CPAID").NotNull();
            this.Property(x => x.ChargedPenalty, "Начислено пени").Column("CCHARGED_PENALTY").NotNull();
            this.Property(x => x.PaidPenalty, "Оплачено пени").Column("CPAID_PENALTY").NotNull();
            this.Property(x => x.BalanceChange, "Сумма по операциям установки/изменения сальдо").Column("BALANCE_CHANGE").DefaultValue(0m).NotNull();
        }
    }
}
