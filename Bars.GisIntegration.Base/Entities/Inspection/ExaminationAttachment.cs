namespace Bars.GisIntegration.Base.Entities.Inspection
{
    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Связь между проверкой и вложением
    /// </summary>
    public class ExaminationAttachment : BaseRisEntity
    {
        /// <summary>
        /// Проверка
        /// </summary>
        public virtual Examination Examination { get; set; }

        /// <summary>
        /// Вложение
        /// </summary>
        public virtual Attachment Attachment { get; set; }
    }
}
