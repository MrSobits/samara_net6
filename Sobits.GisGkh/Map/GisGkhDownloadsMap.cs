namespace Sobits.GisGkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;

    using NHibernate.Mapping.ByCode.Conformist;

    using Sobits.GisGkh.Entities;

    /// <summary>Маппинг для "Sobits.GisGkh.GisGkhDownloads"</summary>
    public class GisGkhDownloadsMap : BaseEntityMap<GisGkhDownloads>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public GisGkhDownloadsMap()
            : base("Sobits.GisGkh.Entities", GisGkhDownloadsMap.TableName)
        {
        }

        public static string TableName => "GIS_GKH_DOWNLOADS";

        public static string SchemaName => "public";

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.Guid, "Guid файла").Column(nameof(GisGkhDownloads.Guid).ToLower()).NotNull();
            this.Property(x => x.EntityT, "Тип сущности").Column(nameof(GisGkhDownloads.EntityT).ToLower()).NotNull();
            this.Property(x => x.RecordId, "Id записи").Column(nameof(GisGkhDownloads.RecordId).ToLower()).NotNull();
            this.Property(x => x.FileField, "Поле для файла").Column(nameof(GisGkhDownloads.FileField).ToLower());
            this.Property(x => x.orgPpaGuid, "orgPpaGuid").Column(nameof(GisGkhDownloads.orgPpaGuid).ToLower());
        }
    }
}