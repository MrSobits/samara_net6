namespace Bars.GisIntegration.Base.Entities.Infrastructure
{
    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Основание управления объектом коммунальной инфраструктуры
    /// </summary>
    public class RisRkiAttachment : BaseRisEntity
    {
        /// <summary>
        /// Объект коммунальной инфраструктуры
        /// </summary>
        public virtual RisRkiItem RkiItem { get; set; }

        /// <summary>
        /// Ссылка на вложение
        /// </summary>
        public virtual Attachment Attachment { get; set; }
    }
}
