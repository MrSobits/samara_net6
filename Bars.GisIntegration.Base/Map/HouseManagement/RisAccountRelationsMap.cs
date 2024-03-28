namespace Bars.GisIntegration.Base.Map.HouseManagement
{
    using Bars.GisIntegration.Base.Map;
    using Entities.HouseManagement;

    /// <summary>
    /// Маппинг для "Bars.Gkh.Ris.Entities.HouseManagement.RisEcnbr"
    /// </summary>
    public class RisAccountRelationsMap : BaseRisEntityMap<RisAccountRelations>
    {
        public RisAccountRelationsMap() :
            base("Bars.Gkh.Ris.Entities.HouseManagement.RisAccountRelations", "RIS_ACCOUNT_RELATIONS")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.Account, "Account").Column("ACCOUNT_ID").Fetch();
            this.Reference(x => x.House, "House").Column("HOUSE_ID").Fetch();
            this.Reference(x => x.ResidentialPremise, "ResidentialPremise").Column("RESIDENTIAL_PREMISE_ID").Fetch();
            this.Reference(x => x.NonResidentialPremises, "NonResidentialPremises").Column("NONRESIDENTIAL_PREMISE_ID").Fetch();
            this.Reference(x => x.LivingRoom, "LivingRoom").Column("LIVING_ROOM_ID").Fetch();
        }
    }
}
