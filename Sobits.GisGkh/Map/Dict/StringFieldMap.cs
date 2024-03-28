namespace Sobits.GisGkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;

    using NHibernate.Mapping.ByCode.Conformist;

    using Sobits.GisGkh.Entities;

    /// <summary>Маппинг для "Sobits.GisGkh.StringField"</summary>
    public class StringFieldMap : BaseEntityMap<StringField>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public StringFieldMap()
            : base("Sobits.GisGkh.Entities", StringFieldMap.TableName)
        {
        }

        public static string TableName => "GIS_GKH_STRING_FIELD";

        public static string SchemaName => "public";

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.NsiItem, "Пункт справочника").Column(nameof(StringField.NsiItem).ToLower()).NotNull().Fetch();
            this.Property(x => x.Name, "Название").Column(nameof(StringField.Name).ToLower()).NotNull();
            this.Property(x => x.Value, "Значение").Column(nameof(StringField.Value).ToLower());
        }
    }

    // ReSharper disable once UnusedMember.Global
    public class StringFieldNhMapping : ClassMapping<StringField>
    {
        public StringFieldNhMapping()
        {
            this.Schema(StringFieldMap.SchemaName);
        }
    }
}