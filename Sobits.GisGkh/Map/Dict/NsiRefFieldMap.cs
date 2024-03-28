namespace Sobits.GisGkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;

    using NHibernate.Mapping.ByCode.Conformist;

    using Sobits.GisGkh.Entities;

    /// <summary>Маппинг для "Sobits.GisGkh.NsiRefField"</summary>
    public class NsiRefFieldMap : BaseEntityMap<NsiRefField>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public NsiRefFieldMap()
            : base("Sobits.GisGkh.Entities", NsiRefFieldMap.TableName)
        {
        }

        public static string TableName => "GIS_GKH_NSI_REF_FIELD";

        public static string SchemaName => "public";

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.NsiItem, "Пункт справочника").Column(nameof(NsiRefField.NsiItem).ToLower()).NotNull().Fetch();
            this.Property(x => x.Name, "Название").Column(nameof(NsiRefField.Name).ToLower()).NotNull();
            this.Property(x => x.RefGUID, "GUID пункта справочника, на который ссылается запись").Column(nameof(NsiRefField.RefGUID).ToLower());
            this.Reference(x => x.NsiRefItem, "Пункт справочника, на который ссылается запись").Column(nameof(NsiRefField.NsiRefItem).ToLower()).Fetch();
        }
    }

    // ReSharper disable once UnusedMember.Global
    public class NsiRefFieldNhMapping : ClassMapping<NsiRefField>
    {
        public NsiRefFieldNhMapping()
        {
            this.Schema(NsiRefFieldMap.SchemaName);
        }
    }
}