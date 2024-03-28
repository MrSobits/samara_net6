namespace Bars.GisIntegration.Base.Entities.CapitalRepair
{
    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Файлы договора
    /// </summary>
    public class RisCrAttachContract : BaseRisEntity
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
