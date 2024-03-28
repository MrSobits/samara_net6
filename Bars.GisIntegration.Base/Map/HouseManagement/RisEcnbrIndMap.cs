namespace Bars.GisIntegration.Base.Map.HouseManagement
{
    using Bars.GisIntegration.Base.Map;
    using Entities.HouseManagement;

    /// <summary>
    /// Маппинг для "Bars.Gkh.Ris.Entities.HouseManagement.RisEcnbrInd"
    /// </summary>
    public class RisEcnbrIndMap : BaseRisEntityMap<RisEcnbrInd>
    {
        public RisEcnbrIndMap() :
            base("Bars.Gkh.Ris.Entities.HouseManagement.RisEcnbrInd", "RIS_ECNBRIND")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.Ecnbr, "Ecnbr").Column("ECNBR_ID").Fetch();
            this.Reference(x => x.Ind, "Ind").Column("IND_ID").Fetch();
        }
    }
}
