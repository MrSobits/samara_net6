namespace Bars.GisIntegration.Base.Map.Inspection
{
    using Bars.GisIntegration.Base.Entities.Inspection;
    using Bars.GisIntegration.Base.Map;

    /// <summary>
    /// Маппинг для "Bars.GisIntegration.Inspection.Entities.CancelPreceptAttachment"
    /// </summary>
    public class CancelPreceptAttachmentMap : BaseRisEntityMap<CancelPreceptAttachment>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public CancelPreceptAttachmentMap()
            : base("Bars.GisIntegration.Inspection.Entities.CancelPreceptAttachment", "GI_CANCEL_PRECEPT_ATTACH")
        {
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.Precept, "Предписание").Column("PRECEPT_ID").NotNull().Fetch();
            this.Reference(x => x.Attachment, "Вложение").Column("ATTACHMENT_ID").NotNull().Fetch();
        }
    }
}
