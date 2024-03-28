namespace Bars.GisIntegration.Base.Map.CapitalRepair
{
    using Bars.GisIntegration.Base.Entities.CapitalRepair;
    using Bars.GisIntegration.Base.Map;

    public class RisCrAttachOutlayMap : BaseRisEntityMap<RisCrAttachOutlay>
    {
        public RisCrAttachOutlayMap()
            : base(
                "Bars.GisIntegration.CapitalReapair.Entities.RisCrAttachOutlay",
                "GI_CR_ATTACHMENT_OUTLAY")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.Contract, "RisCrContract").Column("CONTRACT_ID");
            this.Reference(x => x.Attachment, "RisAttachment").Column("ATTACHMENT_ID");
        }
    }
}
