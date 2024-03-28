namespace Bars.GisIntegration.Base.Map.HouseManagement
{
    using Bars.GisIntegration.Base.Map;
    using Entities.HouseManagement;

    /// <summary>
    /// Маппинг для "Bars.Gkh.Ris.Entities.HouseManagement.RisShareInd"
    /// </summary>
    public class RisShareIndMap : BaseRisEntityMap<RisShareInd>
    {
        public RisShareIndMap() :
            base("Bars.Gkh.Ris.Entities.HouseManagement.RisShareInd", "RIS_SHAREIND")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.Share, "Share").Column("SHARE_ID").Fetch();
            this.Reference(x => x.Ind, "Ind").Column("IND_ID").Fetch();
        }
    }
}
