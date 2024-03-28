namespace Bars.GisIntegration.Base.Map.HouseManagement
{
    using Bars.GisIntegration.Base.Map;
    using Entities.HouseManagement;

    /// <summary>
    /// Маппинг для "Bars.Gkh.Ris.Entities.HouseManagement.RisVoteInitiators"
    /// </summary>
    public class RisVoteInitiatorsMap : BaseRisEntityMap<RisVoteInitiators>
    {
        public RisVoteInitiatorsMap() :
            base("Bars.Gkh.Ris.Entities.HouseManagement.RisVoteInitiators", "RIS_VOTEINITIATORS")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.Ind, "Ind").Column("IND_ID").Fetch();
            this.Reference(x => x.Org, "Org").Column("ORG_ID").Fetch();
            this.Reference(x => x.VotingProtocol, "VotingProtocol").Column("VOTINGPROTOCOL_ID").Fetch();
        }
    }
}
