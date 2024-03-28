namespace Bars.Gkh.Map.EfficiencyRating
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.EfficiencyRating;

    /// <summary>
    /// Маппинг <see cref="AnaliticsGraphManagingOrganization"/>
    /// </summary>
    public class AnaliticsGraphManagingOrganizationMap : PersistentObjectMap<AnaliticsGraphManagingOrganization>
    {
        /// <inheritdoc />
        public AnaliticsGraphManagingOrganizationMap()
            : base("Bars.Gkh.Entities.EfficiencyRating.AnaliticsGraphManagingOrganization", "GKH_EF_ANALITICS_MANORG_REL")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.Graph, "График").Column("ANALITICS_ID");
            this.Reference(x => x.ManagingOrganization, "УО").Column("MANORG_ID");
        }
    }
}