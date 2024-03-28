namespace Sobits.GisGkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;

    using NHibernate.Mapping.ByCode.Conformist;

    using Sobits.GisGkh.Entities;

    /// <summary>Маппинг для "Sobits.GisGkh.OkeiRefField"</summary>
    public class OkeiRefFieldMap : BaseEntityMap<OkeiRefField>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public OkeiRefFieldMap()
            : base("Sobits.GisGkh.Entities", OkeiRefFieldMap.TableName)
        {
        }

        public static string TableName => "GIS_GKH_OKEI_REF_FIELD";

        public static string SchemaName => "public";

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.NsiItem, "Пункт справочника").Column(nameof(OkeiRefField.NsiItem).ToLower()).NotNull().Fetch();
            this.Property(x => x.Name, "Название").Column(nameof(OkeiRefField.Name).ToLower()).NotNull();
            this.Property(x => x.Code, "Ссылка на справочник").Column(nameof(OkeiRefField.Code).ToLower());
        }
    }

    // ReSharper disable once UnusedMember.Global
    public class OkeiRefFieldNhMapping : ClassMapping<OkeiRefField>
    {
        public OkeiRefFieldNhMapping()
        {
            this.Schema(OkeiRefFieldMap.SchemaName);
        }
    }
}