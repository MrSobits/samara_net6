namespace Bars.GisIntegration.Base.Entities.HouseManagement
{
    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Документ новости
    /// </summary>
    public class RisNotificationAttachment : BaseRisEntity
    {
        /// <summary>
        /// Новость
        /// </summary>
        public virtual RisNotification Notification { get; set; }

        /// <summary>
        /// Вложение
        /// </summary>
        public virtual Attachment Attachment { get; set; }
    }
}
