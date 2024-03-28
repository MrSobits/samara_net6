namespace Sobits.GisGkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;

    using NHibernate.Mapping.ByCode.Conformist;

    using Sobits.GisGkh.Entities;

    /// <summary>Маппинг для "Sobits.GisGkh.BooleanField"</summary>
    public class BooleanFieldMap : BaseEntityMap<BooleanField>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public BooleanFieldMap()
            : base("Sobits.GisGkh.Entities", BooleanFieldMap.TableName)
        {
        }

        public static string TableName => "GIS_GKH_BOOLEAN_FIELD";

        public static string SchemaName => "public";

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.NsiItem, "Пункт справочника").Column(nameof(BooleanField.NsiItem).ToLower()).NotNull().Fetch();
            this.Property(x => x.Name, "Название").Column(nameof(BooleanField.Name).ToLower()).NotNull();
            this.Property(x => x.Value, "Значение").Column(nameof(BooleanField.Value).ToLower());
        }
    }

    // ReSharper disable once UnusedMember.Global
    public class BooleanFieldNhMapping : ClassMapping<BooleanField>
    {
        public BooleanFieldNhMapping()
        {
            this.Schema(BooleanFieldMap.SchemaName);
        }
    }
}