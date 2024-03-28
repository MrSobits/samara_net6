namespace Bars.GisIntegration.Base.Map.Infrastructure
{
    using Bars.GisIntegration.Base.Map;
    using Entities.Infrastructure;

    /// <summary>
    /// Маппинг для "Bars.Gkh.Ris.Entities.Infrastructure.RisRkiSource"
    /// </summary>
    public class RisRkiSourceMap : BaseRisEntityMap<RisRkiSource>
    {
        public RisRkiSourceMap() :
            base("Bars.Gkh.Ris.Entities.Infrastructure.RisRkiSource", "ris_source_oki")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.RkiItem, "SourceRki").Column("RKIITEM_ID").Fetch();
            this.Reference(x => x.SourceRkiItem, "SourceRkiItem").Column("SOURCE_RKIITEM_ID").Fetch();
        }
    }
}
