namespace Bars.GisIntegration.Base.Map.CapitalRepair
{
    using Bars.GisIntegration.Base.Entities.CapitalRepair;
    using Bars.GisIntegration.Base.Map;

    public class RisCrAttachContractMap : BaseRisEntityMap<RisCrAttachContract>
    {
        public RisCrAttachContractMap()
            : base(
                "Bars.GisIntegration.CapitalReapair.Entities.RisCrAttachContract",
                "GI_CR_ATTACHMENT_CONTRACT")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.Contract, "RisCrContract").Column("CONTRACT_ID");
            this.Reference(x => x.Attachment, "RisAttachment").Column("ATTACHMENT_ID");
        }
    }
}
