namespace Bars.GisIntegration.Base.Map.HouseManagement
{
    using Bars.GisIntegration.Base.Map;
    using Entities.HouseManagement;

    /// <summary>
    /// Маппинг для "Bars.Gkh.Ris.Entities.HouseManagement.RisVotingProtocol"
    /// </summary>
    public class RisVotingProtocolMap : BaseRisEntityMap<RisVotingProtocol>
    {
        public RisVotingProtocolMap() :
            base("Bars.Gkh.Ris.Entities.HouseManagement.RisVotingProtocol", "RIS_VOTINGPROTOCOL")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.House, "House").Column("HOUSE_ID").Fetch();
            this.Property(x => x.ProtocolNum, "ProtocolNum").Column("PROTOCOLNUM").Length(200);
            this.Property(x => x.ProtocolDate, "ProtocolDate").Column("PROTOCOLDATE");
            this.Property(x => x.VotingPlace, "VotingPlace").Column("VOTINGPLACE").Length(200);
            this.Property(x => x.BeginDate, "BeginDate").Column("BEGINDATE");
            this.Property(x => x.EndDate, "EndDate").Column("ENDDATE");
            this.Property(x => x.Discipline, "Discipline").Column("DISCIPLINE").Length(200);
            this.Property(x => x.MeetingEligibility, "MeetingEligibility").Column("MEETINGELIGIBILITY");
            this.Property(x => x.VotingType, "VotingType").Column("VOTINGTYPE");
            this.Property(x => x.VotingTimeType, "VotingTimeType").Column("VOTINGTIMETYPE");
            this.Property(x => x.Placing, "Placing").Column("PLACE");
            this.Property(x => x.Revert, "Revert").Column("REVERT");
        }
    }
}
