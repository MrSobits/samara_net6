namespace Bars.GkhGji.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities;

    public class GisGmpPatternDictMap : BaseEntityMap<GisGmpPatternDict>
    {
        public GisGmpPatternDictMap() :
            base("Bars.GkhGji.Regions.Tatarstan.Entities.GisGmpPatternDict", "GJI_TAT_GIS_GMP_PATTERN_DICT")
        {

        }

        protected override void Map()
        {
            Property(x => x.PatternName, "PatternName").Column("PATTERN_NAME").Length(500);
            Property(x => x.PatternCode, "PatternCode").Column("PATTERN_CODE").Length(255);
            Property(x => x.Relevance, "Relevance").Column("relevance");
        }
    }
}