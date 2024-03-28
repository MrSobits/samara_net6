namespace Bars.GisIntegration.Base.Map.Inspection
{
    using Bars.GisIntegration.Base.Entities.Inspection;
    using Bars.GisIntegration.Base.Map;

    /// <summary>
    /// Маппинг для "Bars.GisIntegration.Inspection.Entities.Offence"
    /// </summary>
    public class OffenceMap : BaseRisEntityMap<Offence>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public OffenceMap() :
            base("Bars.GisIntegration.Inspection.Entities.Offence", "GI_INSPECTION_OFFENCE")
        {
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.Examination, "Родительская проверка").Column("EXAMINATION_ID").NotNull().Fetch();
            this.Property(x => x.Number, "Номер протокола").Column("NUMBER").Length(50);
            this.Property(x => x.Date, "Дата составления протокола").Column("DATE");
            this.Property(x => x.IsCancelled, "Отменен ли протокол").Column("IS_CANCELLED");
            this.Property(x => x.CancelReason, "Причина отмены документа").Column("CANCEL_REASON");
            this.Property(x => x.CancelDate, "Дата отмены документа").Column("CANCEL_DATE");
            this.Property(x => x.CancelDecisionNumber, "Номер решения об отмене").Column("CANCEL_DECISION_NUM").Length(255);
            this.Property(x => x.IsFulfiledOffence, "IsFulfiledOffence").Column("IS_FULFILED_OFFENCE");
        }
    }
}