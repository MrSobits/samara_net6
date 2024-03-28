namespace Bars.Gkh.Regions.Tatarstan.Map.Egso
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Tatarstan.Entities.Dicts;

    public class EgsoMunicipalityDictMap : PersistentObjectMap<EgsoMunicipalityDict>
    {
        /// <inheritdoc />
        public EgsoMunicipalityDictMap()
            : base("Словарь МО для интеграции с ЕГСО ОВ", "GKH_EGSO_DICT_MUNICIPALITY")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.TerritoryName, "Наименование территории").Column("TERRITORY_NAME").NotNull();
            this.Property(x => x.TerritoryCode, "Код территории").Column("TERRITORY_CODE").NotNull();
            this.Property(x => x.EgsoKey, "Ключ ЕГСО").Column("EGSO_KEY").NotNull();
        }
    }
}