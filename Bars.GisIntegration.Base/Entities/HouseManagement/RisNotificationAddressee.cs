namespace Bars.GisIntegration.Base.Entities.HouseManagement
{
    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Адресат новости
    /// </summary>
    public class RisNotificationAddressee : BaseRisEntity
    {
        /// <summary>
        /// Новость
        /// </summary>
        public virtual RisNotification Notification { get; set; }

        /// <summary>
        /// Адресат (дом)
        /// </summary>
        public virtual RisHouse House { get; set; }
    }
}
