namespace Bars.GisIntegration.Base.Map.HouseManagement
{
    using Bars.GisIntegration.Base.Entities.HouseManagement;
    using Bars.GisIntegration.Base.Map;

    /// <summary>
    /// Маппинг Bars.Gkh.Ris.Entities.RisTrustDocAttachment
    /// </summary>
    public class RisTrustDocAttachmentMap : BaseRisEntityMap<RisTrustDocAttachment>
    {
        public RisTrustDocAttachmentMap()
            :
                base("Bars.Gkh.Ris.Entities.RisTrustDocAttachment", "RIS_TRUSTDOCATTACHMENT")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.PublicPropertyContract, "PublicPropertyContract").Column("PUBLICPROPERTYCONTRACT_ID").Fetch();
            this.Reference(x => x.Attachment, "Attachment").Column("ATTACHMENT_ID").Fetch();
        }
    }
}