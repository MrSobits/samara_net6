namespace Sobits.GisGkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;

    using NHibernate.Mapping.ByCode.Conformist;

    using Sobits.GisGkh.Entities;

    /// <summary>Маппинг для "Sobits.GisGkh.EnumField"</summary>
    public class EnumFieldMap : BaseEntityMap<EnumField>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public EnumFieldMap()
            : base("Sobits.GisGkh.Entities", EnumFieldMap.TableName)
        {
        }

        public static string TableName => "GIS_GKH_ENUM_FIELD";

        public static string SchemaName => "public";

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.NsiItem, "Пункт справочника").Column(nameof(EnumField.NsiItem).ToLower()).NotNull().Fetch();
            this.Property(x => x.Name, "Название").Column(nameof(EnumField.Name).ToLower()).NotNull();
            this.Property(x => x.Position, "Строка со значениями {GUID, value}").Column(nameof(EnumField.Position).ToLower());
        }
    }

    // ReSharper disable once UnusedMember.Global
    public class EnumFieldNhMapping : ClassMapping<EnumField>
    {
        public EnumFieldNhMapping()
        {
            this.Schema(EnumFieldMap.SchemaName);
        }
    }
}