namespace Bars.Gkh.Gis.Map.Kp50
{
    using B4.Modules.Mapping.Mappers;
    using Entities.Kp50;

    /// <summary>Маппинг для "Bars.Gkh.Gis.Entities.Dict.BilllingServiceDictionary"</summary>
    public class BilServiceDictionaryMap : PersistentObjectMap<BilServiceDictionary>
    {

        public BilServiceDictionaryMap() :
            base("Bars.Gkh.Gis.Entities.Dict.BilServiceDictionaryMap", "BIL_DICT_SERVICE")
        {
        }

        protected override void Map()
        {
            Property(x => x.ServiceCode, "ServiceCode").Column("Service_Code").NotNull();
            Property(x => x.ServiceName, "ServiceName").Column("Service_Name").Length(250);

            Property(x => x.ServiceTypeCode, "ServiceTypeCode").Column("Service_Type_Code");
            Property(x => x.ServiceTypeName, "ServiceTypeName").Column("Service_Type_Name").Length(250);

            Property(x => x.MeasureCode, "MeasureCode").Column("Measure_Code");
            Property(x => x.MeasureName, "MeasureName").Column("Measure_Name").Length(250);

            Property(x => x.OrderNumber, "OrderNumber").Column("Order_Number");
            Property(x => x.IsOdnService, "IsOdnService ").Column("Is_Odn_Service ");
            Property(x => x.ParentServiceCode, "ParentServiceCode  ").Column("Parent_Service_Code");

            Reference(x => x.Schema, "Schema").Column("bil_dict_schema_id").NotNull().Fetch();
            Reference(x => x.Service, "ServiceDictionary").Column("gis_service_dictionary_id");
        }
    }
}