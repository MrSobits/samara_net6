namespace Bars.GisIntegration.Base.Map.Infrastructure
{
    using Bars.GisIntegration.Base.Map;
    using Entities.Infrastructure;

    /// <summary>
    /// Маппинг для "Bars.Gkh.Ris.Entities.Infrastructure.RisRkiCommunalService"
    /// </summary>
    public class RisRkiCommunalServiceMap : BaseRisEntityMap<RisRkiCommunalService>
    {
        public RisRkiCommunalServiceMap() :
            base("Bars.Gkh.Ris.Entities.Infrastructure.RisRkiCommunalService", "RIS_RKI_COMMUNAL_SERVICE")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.RkiItem, "RkiItem").Column("RKIITEM_ID").Fetch();
            this.Property(x => x.ServiceCode, "ServiceCode").Column("SERVICE_CODE");
            this.Property(x => x.ServiceGuid, "ServiceGuid").Column("SERVICE_GUID");
            this.Property(x => x.ServiceName, "ServiceName").Column("SERVICE_NAME");
        }
    }
}