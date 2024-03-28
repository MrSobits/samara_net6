namespace Bars.Gkh.Map.EfficiencyRating
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.EfficiencyRating;

    /// <summary>
    /// Маппинг <see cref="AnaliticsGraphMunicipality"/>
    /// </summary>
    public class AnaliticsGraphMunicipalityMap : PersistentObjectMap<AnaliticsGraphMunicipality>
    {
        /// <inheritdoc />
        public AnaliticsGraphMunicipalityMap()
            : base("Bars.Gkh.Entities.EfficiencyRating.AnaliticsGraphMunicipality", "GKH_EF_ANALITICS_MU_REL")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.Graph, "График").Column("ANALITICS_ID");
            this.Reference(x => x.Municipality, "Муниципалитет").Column("MUNICIPALITY_ID");
        }
    }
}