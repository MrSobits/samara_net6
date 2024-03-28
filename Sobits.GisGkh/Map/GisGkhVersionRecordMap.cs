namespace Sobits.GisGkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;

    using NHibernate.Mapping.ByCode.Conformist;

    using Sobits.GisGkh.Entities;

    /// <summary>Маппинг для "Sobits.GisGkh.GisGkhRegionalProgramCR"</summary>
    public class GisGkhVersionRecordMap : BaseEntityMap<GisGkhVersionRecord>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public GisGkhVersionRecordMap()
            : base("Sobits.GisGkh.Entities", GisGkhVersionRecordMap.TableName)
        {
        }

        public static string TableName => "GIS_GKH_VERSION_RECORD";

        public static string SchemaName => "public";

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.GisGkhRegionalProgramCR, "ГИС ЖКХ долгосрочка").Column(nameof(GisGkhVersionRecord.GisGkhRegionalProgramCR).ToLower()).NotNull().Fetch();
            this.Reference(x => x.VersionRecord, "Работа долгосрочки").Column(nameof(GisGkhVersionRecord.VersionRecord).ToLower()).NotNull().Fetch();
            this.Reference(x => x.VersionRecordStage1, "Работа долгосрочки - stage1").Column(nameof(GisGkhVersionRecord.VersionRecordStage1).ToLower()).NotNull().Fetch();
            this.Property(x => x.GisGkhTransportGuid, "ГИС ЖКХ Transport GUID").Column(nameof(GisGkhVersionRecord.GisGkhTransportGuid).ToLower());
            this.Property(x => x.GisGkhGuid, "ГИС ЖКХ GUID").Column(nameof(GisGkhVersionRecord.GisGkhGuid).ToLower());
        }
    }
}