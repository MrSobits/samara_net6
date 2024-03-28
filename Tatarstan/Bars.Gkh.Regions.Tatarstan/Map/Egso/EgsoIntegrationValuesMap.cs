namespace Bars.Gkh.Regions.Tatarstan.Map.Egso
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Tatarstan.Entities.Egso;
    using Bars.Gkh.Regions.Tatarstan.Enums;

    public class EgsoIntegrationValuesMap : PersistentObjectMap<EgsoIntegrationValues>
    {
        public EgsoIntegrationValuesMap() :
            base("Значения", "GKH_EGSO_INTEGRATION_VALUES")
        {      
        }

        protected override void Map()
        {
            this.Reference(x => x.EgsoIntegration, "Задача интеграции с ЕГСО ОВ").Column("EGSO_INTEGRATION_ID").NotNull();
            this.Reference(x => x.MunicipalityDict, "Словарь МО").Column("MUNICIPALITY_DICT_ID").NotNull();
            this.Property(x => x.Value, "Значение").Column("VALUE").NotNull();
        }
    }
}