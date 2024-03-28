namespace Bars.GisIntegration.Base.Map.Infrastructure
{
    using Bars.GisIntegration.Base.Map;
    using Entities.Infrastructure;

    /// <summary>
    /// Маппинг для "Bars.Gkh.Ris.Entities.Infrastructure.RisRkiReceiver"
    /// </summary>
    public class RisRkiReceiverMap : BaseRisEntityMap<RisRkiReceiver>
    {
        public RisRkiReceiverMap() :
            base("Bars.Gkh.Ris.Entities.Infrastructure.RisRkiReceiver", "RIS_RECEIVER_OKI")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.RkiItem, "RkiItem").Column("RKIITEM_ID").Fetch();
            this.Reference(x => x.ReceiverRkiItem, "ReceiverRkiItem").Column("RECEIVER_RKIITEM_ID").Fetch();
        }
    }
}