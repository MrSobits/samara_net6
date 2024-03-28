namespace Bars.GisIntegration.Base.Map.HouseManagement
{
    using Bars.GisIntegration.Base.Map;
    using Entities.HouseManagement;

    public class RisContractAttachmentMap : BaseRisEntityMap<RisContractAttachment>
    {
        public RisContractAttachmentMap() :
            base("Bars.Gkh.Ris.Entities.RisContractAttachment", "RIS_CONTRACTATTACHMENT")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.PublicPropertyContract, "PublicPropertyContract").Column("PUBLICPROPERTYCONTRACT_ID").Fetch();
            this.Reference(x => x.Attachment, "Attachment").Column("ATTACHMENT_ID").Fetch();
            this.Reference(x => x.Contract, "Contract").Column("CONTRACT_ID").Fetch();
            this.Reference(x => x.Charter, "Charter").Column("CHARTER_ID").Fetch();
        }
    }
}
