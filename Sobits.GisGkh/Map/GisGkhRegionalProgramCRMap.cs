namespace Sobits.GisGkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;

    using NHibernate.Mapping.ByCode.Conformist;

    using Sobits.GisGkh.Entities;

    /// <summary>Маппинг для "Sobits.GisGkh.GisGkhRegionalProgramCR"</summary>
    public class GisGkhRegionalProgramCRMap : BaseEntityMap<GisGkhRegionalProgramCR>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public GisGkhRegionalProgramCRMap()
            : base("Sobits.GisGkh.Entities", GisGkhRegionalProgramCRMap.TableName)
        {
        }

        public static string TableName => "GIS_GKH_REGIONAL_PROGRAM_CR";

        public static string SchemaName => "public";

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.GisGkhTransportGuid, "ГИС ЖКХ Transport GUID").Column(nameof(GisGkhRegionalProgramCR.GisGkhTransportGuid).ToLower());
            this.Property(x => x.GisGkhGuid, "ГИС ЖКХ GUID").Column(nameof(GisGkhRegionalProgramCR.GisGkhGuid).ToLower());
            this.Property(x => x.WorkWith, "Признак работы с программой в ГИС ЖКХ").Column(nameof(GisGkhRegionalProgramCR.WorkWith).ToLower()).DefaultValue(false);
        }
    }
}