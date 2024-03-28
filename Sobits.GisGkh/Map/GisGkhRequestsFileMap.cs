namespace Sobits.GisGkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;

    using NHibernate.Mapping.ByCode.Conformist;

    using Sobits.GisGkh.Entities;

    /// <summary>Маппинг для "Sobits.GisGkh.GisGkhRequests"</summary>
    public class GisGkhRequestsFileMap : BaseEntityMap<GisGkhRequestsFile>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public GisGkhRequestsFileMap()
            : base("Sobits.GisGkh.Entities", GisGkhRequestsFileMap.TableName)
        {
        }

        public static string TableName => "GIS_GKH_REQUESTS_FILE";

        public static string SchemaName => "public";

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.GisGkhFileType, "Тип файла").Column(nameof(GisGkhRequestsFile.GisGkhFileType).ToLower()).NotNull();
            this.Reference(x => x.GisGkhRequests, "Запрос в ГИС ЖКХ").Column(nameof(GisGkhRequestsFile.GisGkhRequests).ToLower()).Fetch();
            this.Reference(x => x.FileInfo, "Файл").Column(nameof(GisGkhRequestsFile.FileInfo).ToLower()).Fetch();
        }
    }

    // ReSharper disable once UnusedMember.Global
    public class GisGkhRequestsFileNhMapping : ClassMapping<GisGkhRequestsFile>
    {
        public GisGkhRequestsFileNhMapping()
        {
            this.Schema(GisGkhRequestsFileMap.SchemaName);
        }
    }
}