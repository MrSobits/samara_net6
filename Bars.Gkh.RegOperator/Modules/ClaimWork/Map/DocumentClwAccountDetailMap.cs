namespace Bars.Gkh.RegOperator.Map
{
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;

    public class DocumentClwAccountDetailMap : GkhBaseEntityMap<DocumentClwAccountDetail>
    {
        public DocumentClwAccountDetailMap() : base("CLW_DOCUMENT_ACC_DETAIL")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.DebtBaseTariffSum, "Сумма задолженности по базовому тарифу").Column("DEBT_BASE_TARIFF_SUM").NotNull();
            this.Property(x => x.DebtDecisionTariffSum, "Сумма задолженности по тарифу решения").Column("DEBT_DECISION_TARIFF_SUM").NotNull();
            this.Property(x => x.DebtSum, "Сумма задолженности").Column("DEBT_SUM").NotNull();
            this.Property(x => x.PenaltyDebtSum, "Сумма задолженности по пени").Column("PENALTY_SUM").NotNull();
            this.Property(x => x.PenaltyCalcFormula, "Формула рсчета пени").Column("PENALTY_CALC_FORMULA");
            this.Reference(x => x.PersonalAccount, "Лицевой счет").Column("ACCOUNT_ID").NotNull();
            this.Reference(x => x.Document, "Документ ПИР").Column("DOCUMENT_ID").NotNull();
        }
    }
}