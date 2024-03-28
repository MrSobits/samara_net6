namespace Sobits.GisGkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;

    using NHibernate.Mapping.ByCode.Conformist;

    using Sobits.GisGkh.Entities;

    /// <summary>Маппинг для "Sobits.GisGkh.NsiField"</summary>
    public class NsiFieldMap : BaseEntityMap<NsiField>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public NsiFieldMap()
            : base("Sobits.GisGkh.Entities", NsiFieldMap.TableName)
        {
        }

        public static string TableName => "GIS_GKH_NSI_FIELD";

        public static string SchemaName => "public";

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.NsiItem, "Пункт справочника").Column(nameof(NsiField.NsiItem).ToLower()).NotNull().Fetch();
            this.Property(x => x.Name, "Название").Column(nameof(NsiField.Name).ToLower()).NotNull();
            this.Property(x => x.NsiRegNumber, "Реестровый номер справочника").Column(nameof(NsiField.NsiRegNumber).ToLower()).NotNull();
            this.Reference(x => x.NsiList, "Ссылка на справочник").Column(nameof(NsiField.NsiList).ToLower()).Fetch();
        }
    }

    // ReSharper disable once UnusedMember.Global
    public class NsiFieldNhMapping : ClassMapping<NsiField>
    {
        public NsiFieldNhMapping()
        {
            this.Schema(NsiFieldMap.SchemaName);
        }
    }
}