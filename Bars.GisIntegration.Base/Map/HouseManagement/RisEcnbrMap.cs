namespace Bars.GisIntegration.Base.Map.HouseManagement
{
    using Bars.GisIntegration.Base.Map;
    using Entities.HouseManagement;

    /// <summary>
    /// Маппинг для "Bars.Gkh.Ris.Entities.HouseManagement.RisEcnbr"
    /// </summary>
    public class RisEcnbrMap : BaseRisEntityMap<RisEcnbr>
    {
        public RisEcnbrMap() :
            base("Bars.Gkh.Ris.Entities.HouseManagement.RisEcnbr", "RIS_ECNBR")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.EndDate, "EndDate").Column("ENDDATE");
            this.Property(x => x.KindCode, "KindCode").Column("KINDCODE").Length(50);
            this.Property(x => x.KindGuid, "KindGuid").Column("KINDGUID").Length(50);
            this.Reference(x => x.RisEcnbrContragent, "Contragent").Column("CONTRAGENT_ID").Fetch();
            this.Reference(x => x.Share, "Share").Column("SHARE_ID").Fetch();
        }
    }
}
