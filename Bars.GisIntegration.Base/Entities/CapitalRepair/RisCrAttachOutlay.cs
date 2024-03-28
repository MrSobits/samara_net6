namespace Bars.GisIntegration.Base.Entities.CapitalRepair
{
    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Файлы сметной документации
    /// </summary>
    public class RisCrAttachOutlay : BaseRisEntity
    {
        /// <summary>
        /// Импортируемый договор
        /// </summary>
        public virtual RisCrContract Contract { get; set; }

        /// <summary>
        /// Файл-вложение
        /// </summary>
        public virtual Attachment Attachment { get; set; }
    }
}
