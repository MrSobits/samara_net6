namespace Bars.GisIntegration.Base.Map.Inspection
{
    using Bars.GisIntegration.Base.Entities.Inspection;
    using Bars.GisIntegration.Base.Map;

    /// <summary>
    /// Маппинг для "Bars.GisIntegration.Inspection.Entities.OffenceAttachment"
    /// </summary>
    public class OffenceAttachmentMap : BaseRisEntityMap<OffenceAttachment>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public OffenceAttachmentMap()
            : base("Bars.GisIntegration.Inspection.Entities.OffenceAttachment", "GI_INSPECTION_OFFENCE_ATTACH")
        {
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.Offence, "Протокол").Column("OFFENCE_ID").NotNull().Fetch();
            this.Reference(x => x.Attachment, "Вложение").Column("ATTACHMENT_ID").NotNull().Fetch();
        }
    }
}
