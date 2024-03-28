namespace Bars.GisIntegration.Base.Map.HouseManagement
{
    using Bars.GisIntegration.Base.Map;
    using Entities.HouseManagement;

    /// <summary>
    /// Маппинг для "Bars.Gkh.Ris.Entities.HouseManagement.RisShareEncbrLivingHouse"
    /// </summary>
    public class RisShareEncbrLivingHouseMap : BaseRisEntityMap<RisShareEncbrLivingHouse>
    {
        public RisShareEncbrLivingHouseMap() :
            base("Bars.Gkh.Ris.Entities.HouseManagement.RisShareEncbrLivingHouse", "RIS_SHAREECNBRLIVINGHOUSE")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.IntPart, "IntPart").Column("INTPART").Length(50);
            this.Property(x => x.FracPart, "FracPart").Column("FRACPART").Length(50);
            this.Reference(x => x.Ecnbr, "Ecnbr").Column("ECNBR_ID").Fetch();
            this.Reference(x => x.Share, "Share").Column("SHARE_ID").Fetch();
            this.Reference(x => x.House, "House").Column("HOUSE_ID").Fetch();
        }
    }
}
