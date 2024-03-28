namespace Bars.GisIntegration.Base.Entities.Infrastructure
{
    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Документы, подтверждающие соответствие требованиям энергетической эффективности
    /// </summary>
    public class RisAttachmentsEnergyEfficiency : BaseRisEntity
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