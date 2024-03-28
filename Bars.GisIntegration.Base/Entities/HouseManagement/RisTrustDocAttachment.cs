namespace Bars.GisIntegration.Base.Entities.HouseManagement
{
    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Документы, подтверждающие полномочия заключать договор
    /// </summary>
    public class RisTrustDocAttachment : BaseRisEntity
    {
        /// <summary>
        /// Ссылка на ДОИ
        /// </summary>
        public virtual RisPublicPropertyContract PublicPropertyContract { get; set; }

        /// <summary>
        /// Ссылка на вложение
        /// </summary>
        public virtual Attachment Attachment { get; set; }
    }
}
