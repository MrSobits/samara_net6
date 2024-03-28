namespace Bars.Gkh.Gis.Map.Dict
{
    using B4.Modules.Mapping.Mappers;
    using Entities.Dict;

    public class GisNormativDictMap : BaseEntityMap<GisNormativDict>
    {
        public GisNormativDictMap() : 
                base("Bars.Gkh.Gis.Entities.Dict.GisNormativDict", "GIS_NORMATIV_DICTIONARY")
        {
        }

        protected override void Map()
        {
            Reference(x => x.Municipality, "Municipality").Column("MU_ID").Fetch();
            Reference(x => x.Service, "Service").Column("SERVICE_ID").Fetch();
            Property(x => x.Value, "Value").Column("VALUE");
            Property(x => x.Measure, "Measure").Column("MEASURE").Length(50);
            Property(x => x.DateStart, "DateStart").Column("DATE_START").NotNull();
            Property(x => x.DateEnd, "DateEnd").Column("DATE_END");
            Property(x => x.Description, "Description").Column("DESCRIPTION").Length(500);
            Property(x => x.DocumentName, "DocumentName").Column("DOC_NAME").Length(100);
            Reference(x => x.DocumentFile, "DocumentFile").Column("DOC_FILE_ID").Fetch();
        }
    }
}