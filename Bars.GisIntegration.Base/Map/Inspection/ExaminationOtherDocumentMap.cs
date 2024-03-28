namespace Bars.GisIntegration.Base.Map.Inspection
{
    using Bars.GisIntegration.Base.Entities.Inspection;
    using Bars.GisIntegration.Base.Map;

    /// <summary>
    /// Маппинг для "Bars.GisIntegration.Inspection.Entities.ExaminationOtherDocument"
    /// </summary>
    public class ExaminationOtherDocumentMap : BaseRisEntityMap<ExaminationOtherDocument>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public ExaminationOtherDocumentMap()
            : base("Bars.GisIntegration.Inspection.Entities.ExaminationOtherDocument", "GI_EXAMINATION_OTHER_DOCUMENT")
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
