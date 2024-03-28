namespace Bars.GisIntegration.Base.Entities.Inspection
{
    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Другие документы проверки
    /// </summary>
    public class ExaminationOtherDocument : BaseRisEntity
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
