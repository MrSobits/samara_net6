namespace Sobits.GisGkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;

    using NHibernate.Mapping.ByCode.Conformist;

    using Sobits.GisGkh.Entities;

    /// <summary>Маппинг для "Sobits.GisGkh.DateField"</summary>
    public class DateFieldMap : BaseEntityMap<DateField>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public DateFieldMap()
            : base("Sobits.GisGkh.Entities", DateFieldMap.TableName)
        {
        }

        public static string TableName => "GIS_GKH_DATE_FIELD";

        public static string SchemaName => "public";

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.NsiItem, "Пункт справочника").Column(nameof(DateField.NsiItem).ToLower()).NotNull().Fetch();
            this.Property(x => x.Name, "Название").Column(nameof(DateField.Name).ToLower()).NotNull();
            this.Property(x => x.Value, "Значение").Column(nameof(DateField.Value).ToLower());
        }
    }

    // ReSharper disable once UnusedMember.Global
    public class DateFieldNhMapping : ClassMapping<DateField>
    {
        public DateFieldNhMapping()
        {
            this.Schema(DateFieldMap.SchemaName);
        }
    }
}