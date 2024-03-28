namespace Bars.GisIntegration.Base.Map.Inspection
{
    using Bars.GisIntegration.Base.Entities.Inspection;
    using Bars.GisIntegration.Base.Map;

    /// <summary>
    /// Маппинг для "Bars.GisIntegration.Inspection.Entities.ExaminationAttachment"
    /// </summary>
    public class ExaminationAttachmentMap : BaseRisEntityMap<ExaminationAttachment>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public ExaminationAttachmentMap()
            : base("Bars.GisIntegration.Inspection.Entities.ExaminationAttachment", "GI_INSPECTION_EXAM_ATTACH")
        {
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.Examination, "Проверка").Column("EXAMINATION_ID").NotNull().Fetch();
            this.Reference(x => x.Attachment, "Вложение").Column("ATTACHMENT_ID").NotNull().Fetch();
        }
    }
}
