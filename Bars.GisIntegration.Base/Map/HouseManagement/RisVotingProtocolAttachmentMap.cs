namespace Bars.GisIntegration.Base.Map.HouseManagement
{
    using Bars.GisIntegration.Base.Map;
    using Entities.HouseManagement;

    public class RisVotingProtocolAttachmentMap : BaseRisEntityMap<RisVotingProtocolAttachment>
    {
        public RisVotingProtocolAttachmentMap() :
            base("Bars.Gkh.Ris.Entities.RisVotingProtocolAttachment", "RIS_VOTINGPROTOCOL_ATTACHMENT")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.VotingProtocol, "VotingProtocol").Column("VOTINGPROTOCOL_ID").Fetch();
            this.Reference(x => x.Attachment, "Attachment").Column("ATTACHMENT_ID").Fetch();
        }
    }
}
