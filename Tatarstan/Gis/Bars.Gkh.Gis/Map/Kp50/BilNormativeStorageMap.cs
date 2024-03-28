namespace Bars.Gkh.Gis.Map.Kp50
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Gis.Entities.Kp50;

    /// <summary>Маппинг для "Bars.Gkh.Gis.Entities.Kp50.BilNormativStorage"</summary>
    public class BilNormativStorageMap : PersistentObjectMap<BilNormativStorage>
    {

        public BilNormativStorageMap() :
            base("Bars.Gkh.Gis.Entities.Kp50.BilNormativStorage", "BIL_NORMATIVE_STORAGE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.BilService, "BilService").Column("BIL_DICT_SERVICE_ID").Fetch();
            Property(x => x.NormativeTypeCode, "NormativeTypeCode").Column("NORMATIVE_TYPE_CODE");
            Property(x => x.NormativeTypeName, "NormativeTypeName").Column("NORMATIVE_TYPE_NAME").Length(200);
            Property(x => x.NormativeCode, "NormativeCode").Column("NORMATIVE_CODE");
            Property(x => x.NormativeName, "NormativeName").Column("NORMATIVE_NAME").Length(200);
            Property(x => x.NormativeDescription, "NormativeDescription").Column("NORMATIVE_DESCRIPTION").Length(200);
            Property(x => x.NormativeValue, "NormativeValue").Column("NORMATIVE_VALUE");
            Property(x => x.NormativeStartDate, "NormativeStartDate").Column("NORMATIVE_START_DATE");
            Property(x => x.NormativeEndDate, "NormativeEndDate").Column("NORMATIVE_END_DATE");
        }
    }
}