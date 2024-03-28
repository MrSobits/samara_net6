namespace Bars.GisIntegration.Base.Map.HouseManagement
{
    using Bars.GisIntegration.Base.Map;
    using Entities.HouseManagement;

    /// <summary>
    /// Маппинг для "Bars.Gkh.Ris.Entities.HouseManagement.SupplyResourceContract"
    /// </summary>
    public class SupResContractAttachmentMap : BaseRisEntityMap<SupResContractAttachment>
    {
        public SupResContractAttachmentMap() :
            base("Bars.Gkh.Ris.Entities.HouseManagement.SupResContractAttachment", "SUP_RES_CONTRACT_ATTACHMENT")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.Contract, "Contract").Column("CONTRACT_ID").Fetch();
            this.Reference(x => x.Attachment, "Attachment").Column("ATTACHMENT_ID").Fetch();
        }
    }
}
