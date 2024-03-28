namespace Sobits.GisGkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;

    using NHibernate.Mapping.ByCode.Conformist;

    using Sobits.GisGkh.Entities;

    /// <summary>Маппинг для "Sobits.GisGkh.GisGkhRequests"</summary>
    public class NsiListMap : BaseEntityMap<NsiList>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public NsiListMap()
            : base("Sobits.GisGkh.Entities", NsiListMap.TableName)
        {
        }

        public static string TableName => "GIS_GKH_NSI_LIST";

        public static string SchemaName => "public";

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.ListGroup, "Группа справочников").Column(nameof(NsiList.ListGroup).ToLower()).NotNull();
            this.Property(x => x.GisGkhName, "Наименование справочника в ГИС ЖКХ").Column(nameof(NsiList.GisGkhName).ToLower()).NotNull();
            this.Property(x => x.GisGkhCode, "Код справочника в ГИС ЖКХ").Column(nameof(NsiList.GisGkhCode).ToLower()).NotNull();
            this.Property(x => x.EntityName, "Наименование справочника в системе").Column(nameof(NsiList.EntityName).ToLower());
            this.Property(x => x.RefreshDate, "Дата обновления справочника из ГИС ЖКХ").Column(nameof(NsiList.RefreshDate).ToLower());
            this.Property(x => x.MatchDate, "Дата сопоставления справочника").Column(nameof(NsiList.MatchDate).ToLower());
            this.Property(x => x.ModifiedDate, "Актуальность справочника ГИС ЖКХ").Column(nameof(NsiList.ModifiedDate).ToLower());
            this.Reference(x => x.Contragent, "Контрагент (владелец справочника)").Column(nameof(NsiList.Contragent).ToLower()).Fetch();
        }
    }

    // ReSharper disable once UnusedMember.Global
    public class NsiListNhMapping : ClassMapping<NsiList>
    {
        public NsiListNhMapping()
        {
            this.Schema(NsiListMap.SchemaName);
        }
    }
}