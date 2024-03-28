namespace Bars.GisIntegration.Base.Map.Inspection
{
    using Bars.GisIntegration.Base.Entities.Inspection;
    using Bars.GisIntegration.Base.Map;

    /// <summary>
    /// Маппинг для "Bars.GisIntegration.Inspection.Entities.Precept"
    /// </summary>
    public class PreceptMap : BaseRisEntityMap<Precept>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public PreceptMap() :
            base("Bars.GisIntegration.Inspection.Entities.Precept", "GI_INSPECTION_PRECEPT")
        {
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.Examination, "Родительская проверка").Column("EXAMINATION_ID").NotNull().Fetch();
            this.Property(x => x.Number, "Номер документа").Column("NUMBER").Length(50);
            this.Property(x => x.Date, "Дата документа").Column("DATE");
            this.Property(x => x.FiasHouseGuid, "Адрес дома").Column("FIAS_HOUSE_GUID").Length(200);
            this.Property(x => x.IsCancelled, "Отменено ли предписание").Column("IS_CANCELLED");
            this.Property(x => x.CancelReason, "Причина отмены документа").Column("CANCEL_REASON");
            this.Property(x => x.CancelDate, "Дата отмены документа").Column("CANCEL_DATE");
            this.Property(x => x.Deadline, "Deadline").Column("DEADLINE");
            this.Property(x => x.IsFulfiledPrecept, "IsFulfiledPrecept").Column("IS_FULFILED_PRECEPT");
            this.Property(x => x.CancelReasonGuid, "CancelReasonGuid").Column("CANCEL_REASON_GUID");
            this.Property(x => x.OrgRootEntityGuid, "OrgRootEntityGuid").Column("ORG_ROOT_ENTITY_GUID");
            this.Property(x => x.IsCancelledAndIsFulfiled, "IsCancelledAndIsFulfiled").Column("IS_CANCELLED_AND_IS_FULFILED");
        }
    }
}