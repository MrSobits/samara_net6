namespace Bars.Gkh.ClaimWork.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.ClaimWork.Entities;

    /// <summary>
    /// Маппинг для "Реструктуризация долга"
    /// </summary>
    public class RestructDebtMap : JoinedSubClassMap<RestructDebt>
    {
        
        public RestructDebtMap() : base("Реструктуризация долга", "CLW_RESTRUCT_DEBT")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.DocumentState, "Статус договора").Column("DOCUMENT_STATE");
            this.Property(x => x.BaseTariffDebtSum, "Сумма задолженности по базовому тарифу").Column("BASE_TARIFF_DEBT_SUM");
            this.Property(x => x.DecisionTariffDebtSum, "Сумма задолженности по тарифу решения").Column("DECISION_TARIFF_DEBT_SUM");
            this.Property(x => x.DebtSum, "Сумма задолженности").Column("DEBT_SUM");
            this.Property(x => x.PenaltyDebtSum, "Сумма задолженности по пени").Column("PENALTY_DEBT_SUM");
            this.Property(x => x.RestructSum, "Сумма реструктуризации").Column("RESTRUCT_SUM");
            this.Property(x => x.PercentSum, "В т. ч. проценты").Column("PERCENT_SUM");
            this.Property(x => x.TerminationDate, "Дата расторжения").Column("TERMINATION_DATE");
            this.Property(x => x.TerminationNumber, "Номер документа").Column("TERMINATION_NUMBER");
            this.Property(x => x.TerminationReason, "Причина").Column("TERMINATION_REASON");
            this.Property(x => x.Reason, "Причина").Column("REASON");

            this.Reference(x => x.DocFile, "Файл документа").Column("DOC_FILE_ID");
            this.Reference(x => x.PaymentScheduleFile, "Файл графика платежей").Column("SCHEDULE_FILE_ID");
            this.Reference(x => x.TerminationFile, "Документ-основание").Column("TERMINATION_FILE_ID");
        }
    }
}
