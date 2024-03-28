namespace Bars.GisIntegration.Base.Map.HouseManagement
{
    using Bars.GisIntegration.Base.Map;
    using Entities.HouseManagement;

    /// <summary>
    /// Маппинг для "Bars.Gkh.Ris.Entities.HouseManagement.RisShareEncbrLivingRoom"
    /// </summary>
    public class RisShareEncbrLivingRoomMap : BaseRisEntityMap<RisShareEncbrLivingRoom>
    {
        public RisShareEncbrLivingRoomMap() :
            base("Bars.Gkh.Ris.Entities.HouseManagement.RisShareEncbrLivingRoom", "RIS_SHAREECNBRLIVINGROOM")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.IntPart, "IntPart").Column("INTPART").Length(50);
            this.Property(x => x.FracPart, "FracPart").Column("FRACPART").Length(50);
            this.Reference(x => x.Ecnbr, "Ecnbr").Column("ECNBR_ID").Fetch();
            this.Reference(x => x.Share, "Share").Column("SHARE_ID").Fetch();
            this.Reference(x => x.LivingRoom, "LivingRoom").Column("LIVINGROOM_ID").Fetch();
        }
    }
}
