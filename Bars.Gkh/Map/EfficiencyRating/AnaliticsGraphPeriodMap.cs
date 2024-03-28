namespace Bars.Gkh.Map.EfficiencyRating
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.EfficiencyRating;

    /// <summary>
    /// Маппинг <see cref="AnaliticsGraphPeriod"/>
    /// </summary>
    public class AnaliticsGraphPeriodMap : PersistentObjectMap<AnaliticsGraphPeriod>
    {
        /// <inheritdoc />
        public AnaliticsGraphPeriodMap()
            : base("Bars.Gkh.Entities.EfficiencyRating.AnaliticsGraphPeriod", "GKH_EF_ANALITICS_PERIOD_REL")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.Graph, "График").Column("ANALITICS_ID");
            this.Reference(x => x.Period, "Период").Column("PERIOD_ID");
        }
    }
}