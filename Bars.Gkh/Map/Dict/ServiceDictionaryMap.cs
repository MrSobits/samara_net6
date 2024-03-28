namespace Bars.Gkh.Map.Dict
{
    using Bars.Gkh.Entities.Dicts;

    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Bars.Gkh.Gis.Entities.Dict.ServiceDictionary"</summary>
    public class GisServiceDictionaryMap : BaseImportableEntityMap<ServiceDictionary>
    {
        public GisServiceDictionaryMap() :
                base("Bars.Gkh.Gis.Entities.Dict.ServiceDictionary", "GIS_SERVICE_DICTIONARY")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.ExportId, "ExportId").Column("EXPORT_ID").NotNull();
            this.Property(x => x.Code, "Code").Column("CODE");
            this.Property(x => x.Name, "Name").Column("NAME").Length(250);
            this.Property(x => x.Measure, "Measure").Column("MEASURE").Length(250);
            this.Property(x => x.TypeService, "TypeService").Column("TYPE_SERVICE").NotNull();
            this.Property(x => x.TypeCommunalResource, "TypeCommunalResource").Column("TYPE_COMM_RESOURCE");
            this.Property(x => x.IsProvidedForAllHouseNeeds, "IsProvidedForAllHouseNeeds").Column("FOR_ALL_HOUSE_NEEDS");
            this.Reference(x => x.UnitMeasure, "Ед. измерения").Column("UNIT_MEASURE_ID").Fetch();
        }
    }

    /// <summary>ReadOnly ExportId</summary>
    public class ServiceDictionaryNhMapping : BaseHaveExportIdMapping<ServiceDictionary>
    {
    }
}