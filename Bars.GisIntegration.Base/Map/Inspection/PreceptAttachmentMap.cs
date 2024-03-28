namespace Bars.GisIntegration.Base.Map.Inspection
{
    using Bars.GisIntegration.Base.Entities.Inspection;
    using Bars.GisIntegration.Base.Map;

    /// <summary>
    /// Маппинг для "Bars.GisIntegration.Inspection.Entities.PreceptAttachment"
    /// </summary>
    public class PreceptAttachmentMap : BaseRisEntityMap<PreceptAttachment>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public PreceptAttachmentMap()
            : base("Bars.GisIntegration.Inspection.Entities.PreceptAttachment", "GI_INSPECTION_PRECEPT_ATTACH")
        {
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.Precept, "Предписание").Column("PRECEPT_ID").NotNull().Fetch();
            this.Reference(x => x.Attachment, "Вложение").Column("ATTACHMENT_ID").NotNull().Fetch();
            this.Property(x => x.IsCancelAttachment, "Является ли вложение документом отмены").Column("IS_CANCEL_ATTACH");
        }
    }
}
