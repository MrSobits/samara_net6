namespace Sobits.GisGkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;

    using NHibernate.Mapping.ByCode.Conformist;

    using Sobits.GisGkh.Entities;

    /// <summary>Маппинг для "Sobits.GisGkh.FiasAddressRefField"</summary>
    public class FiasAddressRefFieldMap : BaseEntityMap<FiasAddressRefField>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public FiasAddressRefFieldMap()
            : base("Sobits.GisGkh.Entities", FiasAddressRefFieldMap.TableName)
        {
        }

        public static string TableName => "GIS_GKH_FIAS_ADDRESS_REF_FIELD";

        public static string SchemaName => "public";

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.NsiItem, "Пункт справочника").Column(nameof(FiasAddressRefField.NsiItem).ToLower()).NotNull().Fetch();
            this.Property(x => x.Name, "Название").Column(nameof(FiasAddressRefField.Name).ToLower()).NotNull();
            this.Property(x => x.FiasGUID, "Строка с GUID справочника ФИАС").Column(nameof(FiasAddressRefField.FiasGUID).ToLower());
        }
    }

    // ReSharper disable once UnusedMember.Global
    public class FiasAddressRefFieldNhMapping : ClassMapping<FiasAddressRefField>
    {
        public FiasAddressRefFieldNhMapping()
        {
            this.Schema(FiasAddressRefFieldMap.SchemaName);
        }
    }
}