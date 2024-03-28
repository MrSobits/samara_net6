namespace Bars.GisIntegration.Base.Map.HouseManagement
{
    using Bars.GisIntegration.Base.Map;
    using Entities.HouseManagement;

    /// <summary>
    /// Маппинг для "Bars.Gkh.Ris.Entities.HouseManagement.RisAccount"
    /// </summary>
    public class RisAccountMap : BaseRisEntityMap<RisAccount>
    {
        public RisAccountMap() :
            base("Bars.Gkh.Ris.Entities.HouseManagement.RisAccount", "RIS_ACCOUNT")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.RisAccountType, "RisAccountType").Column("TYPEACCOUNT");
            this.Reference(x => x.OwnerInd, "OwnerInd").Column("OWNERIND_ID").Fetch();
            this.Reference(x => x.OwnerOrg, "OwnerOrg").Column("OWNERORG_ID").Fetch();
            this.Reference(x => x.RenterInd, "RenterInd").Column("RENTERIND_ID").Fetch();
            this.Reference(x => x.RenterOrg, "RenterOrg").Column("RENTERORG_ID").Fetch();
            this.Property(x => x.LivingPersonsNumber, "LivingPersonsNumber").Column("LIVINGPERSONSNUMBER");
            this.Property(x => x.TotalSquare, "TotalSquare").Column("TOTALSQUARE");
            this.Property(x => x.ResidentialSquare, "ResidentialSquare").Column("RESIDENTIALSQUARE");
            this.Property(x => x.HeatedArea, "HeatedArea").Column("HEATEDAREA");
            this.Property(x => x.Closed, "Closed").Column("CLOSED");
            this.Property(x => x.CloseReasonCode, "CloseReasonCode").Column("CLOSEREASON_CODE").Length(50);
            this.Property(x => x.CloseReasonGuid, "CloseReasonGuid").Column("CLOSEREASON_GUID").Length(50);
            this.Property(x => x.CloseDate, "CloseDate").Column("CLOSEDATE");
            this.Property(x => x.BeginDate, "BeginDate").Column("BEGINDATE");
            this.Property(x => x.AccountNumber, "AccountNumber").Column("ACCOUNTNUMBER").Length(50);
            this.Property(x => x.IsRenter, "IsRenter").Column("IS_RENTER");
        }
    }
}