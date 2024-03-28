namespace Bars.GisIntegration.Base.Entities.HouseManagement
{
    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Документы договора
    /// </summary>
    public class RisContractAttachment : BaseRisEntity
    {
        /// <summary>
        /// ДОИ
        /// </summary>
        public virtual RisPublicPropertyContract PublicPropertyContract { get; set; }

        /// <summary>
        /// Вложение
        /// </summary>
        public virtual Attachment Attachment { get; set; }

        /// <summary>
        /// Договор управления
        /// </summary>
        public virtual RisContract Contract { get; set; }

        /// <summary>
        /// Устав
        /// </summary>
        public virtual Charter Charter { get; set; }
    }
}
