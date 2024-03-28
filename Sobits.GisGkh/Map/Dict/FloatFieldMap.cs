namespace Sobits.GisGkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;

    using NHibernate.Mapping.ByCode.Conformist;

    using Sobits.GisGkh.Entities;

    /// <summary>Маппинг для "Sobits.GisGkh.FloatField"</summary>
    public class FloatFieldMap : BaseEntityMap<FloatField>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public FloatFieldMap()
            : base("Sobits.GisGkh.Entities", FloatFieldMap.TableName)
        {
        }

        public static string TableName => "GIS_GKH_FLOAT_FIELD";

        public static string SchemaName => "public";

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.NsiItem, "Пункт справочника").Column(nameof(FloatField.NsiItem).ToLower()).NotNull().Fetch();
            this.Property(x => x.Name, "Название").Column(nameof(FloatField.Name).ToLower()).NotNull();
            this.Property(x => x.Value, "Значение").Column(nameof(FloatField.Value).ToLower());
        }
    }

    // ReSharper disable once UnusedMember.Global
    public class FloatFieldNhMapping : ClassMapping<FloatField>
    {
        public FloatFieldNhMapping()
        {
            this.Schema(FloatFieldMap.SchemaName);
        }
    }
}