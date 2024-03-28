namespace Sobits.GisGkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;

    using NHibernate.Mapping.ByCode.Conformist;

    using Sobits.GisGkh.Entities;

    /// <summary>Маппинг для "Sobits.GisGkh.IntegerField"</summary>
    public class IntegerFieldMap : BaseEntityMap<IntegerField>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public IntegerFieldMap()
            : base("Sobits.GisGkh.Entities", IntegerFieldMap.TableName)
        {
        }

        public static string TableName => "GIS_GKH_INTEGER_FIELD";

        public static string SchemaName => "public";

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.NsiItem, "Пункт справочника").Column(nameof(IntegerField.NsiItem).ToLower()).NotNull().Fetch();
            this.Property(x => x.Name, "Название").Column(nameof(IntegerField.Name).ToLower()).NotNull();
            this.Property(x => x.Value, "Значение").Column(nameof(IntegerField.Value).ToLower());
        }
    }

    // ReSharper disable once UnusedMember.Global
    public class IntegerFieldNhMapping : ClassMapping<IntegerField>
    {
        public IntegerFieldNhMapping()
        {
            this.Schema(IntegerFieldMap.SchemaName);
        }
    }
}