namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Regions.Chelyabinsk.Entities;

    /// <summary>Маппинг для "Должника Агент ПИР"</summary>
    public class AgentPIRDebtorMap : BaseEntityMap<AgentPIRDebtor>
    {

        public AgentPIRDebtorMap() :
                base("Должник агент ПИР", "AGENT_PIR_DEBTOR")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.AgentPIR, "Агент ПИР").Column("AGENT_PIR_ID");
            this.Reference(x => x.BasePersonalAccount, "ЛС").Column("AGENT_PIR_DEBTOR_PA_ID");
            this.Property(x => x.Status, "Статус задолжности").Column("AGENT_PIR_DEBTOR_STATUS");
            this.Property(x=> x.CustomDate, "CustomDate").Column("CUSTOM_DATE");
            this.Property(x => x.UseCustomDate, "CustomDate").Column("USE_CUSTOM_DATE");
            this.Property(x => x.DebtBaseTariff, "DebtBaseTariff").Column("BT_DEBT");
            this.Property(x => x.PenaltyDebt, "PenaltyDebt").Column("PENALTY_DEBT");
            this.Property(x => x.DebtStartDate, "DebtStartDate").Column("START_DATE");
            this.Property(x => x.DebtEndDate, "DebtEndDate").Column("END_DATE");
            this.Property(x => x.DebtCalc, "Способ погашения задолженности").Column("DEBT_CALC");
            this.Property(x => x.PenaltyCharge, "Способ начисления пени").Column("PENALTY_CHARGE");
            this.Reference(x => x.Ordering, "ЛС").Column("ORDERING_FILE_ID");
        }
    }
}
