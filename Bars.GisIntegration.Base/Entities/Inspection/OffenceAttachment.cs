namespace Bars.GisIntegration.Base.Entities.Inspection
{
    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Связь между протоколом и вложением
    /// </summary>
    public class OffenceAttachment : BaseRisEntity
    {
        /// <summary>
        /// Протокол
        /// </summary>
        public virtual Offence Offence { get; set; }

        /// <summary>
        /// Вложение
        /// </summary>
        public virtual Attachment Attachment { get; set; }
    }
}
