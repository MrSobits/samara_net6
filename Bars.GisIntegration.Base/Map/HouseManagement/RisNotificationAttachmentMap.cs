namespace Bars.GisIntegration.Base.Map.HouseManagement
{
    using Bars.GisIntegration.Base.Map;
    using Entities.HouseManagement;

    /// <summary>
    /// Маппинг для "Bars.Gkh.Ris.Entities.HouseManagement.RisNotificationAttachment"
    /// </summary>
    public class RisNotificationAttachmentMap : BaseRisEntityMap<RisNotificationAttachment>
    {
        public RisNotificationAttachmentMap() :
            base("Bars.Gkh.Ris.Entities.HouseManagement.RisNotificationAttachment", "RIS_NOTIFICATION_ATTACHMENT")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.Attachment, "Attachment").Column("ATTACHMENT_ID");
            this.Reference(x => x.Notification, "Notification").Column("NOTIFICATION_ID");
        }
    }
}
