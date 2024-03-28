namespace Bars.Gkh.Gis.Map.Kp50
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Gis.Entities.Kp50;


    /// <summary>Маппинг для "Bars.Gkh.Gis.Entities.Kp50.BilTarifStorage"</summary>
    public class BilTarifStorageMap : PersistentObjectMap<BilTarifStorage>
    {

        public BilTarifStorageMap() :
            base("Bars.Gkh.Gis.Entities.Kp50.BilTarifStorage", "BIL_TARIF_STORAGE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.BilService, "BilService").Column("BIL_DICT_SERVICE_ID").Fetch();
            Reference(x => x.RealityObject, "RealityObject").Column("GKH_REALITY_OBJECT_ID").Fetch();
            Property(x => x.SupplierCode, "SupplierCode").Column("Supplier_Code");
            Property(x => x.SupplierName, "SupplierName").Column("Supplier_Name").Length(200);

            Property(x => x.FormulaCode, "FormulaCode").Column("Formula_Code");
            Property(x => x.FormulaTypeCode, "FormulaTypeCode").Column("Formula_Type_Code");
            Property(x => x.FormulaName, "FormulaName").Column("Formula_Name").Length(200);


            Property(x => x.TarifCode, "TarifCode").Column("Tarif_Code");
            Property(x => x.TarifName, "TarifName").Column("Tarif_Name").Length(200);

            Property(x => x.TarifTypeCode, "TarifTypeCode").Column("Tarif_Type_Code");
            Property(x => x.TarifTypeName, "TarifTypeName").Column("Tarif_Type_Name").Length(200);

            Property(x => x.TarifValue, "TarifValue").Column("Tarif_Value");
            Property(x => x.TarifStartDate, "TarifStartDate").Column("Tarif_Start_Date");
            Property(x => x.TarifEndDate, "TarifEndDate").Column("Tarif_End_Date");

            Property(x => x.LsCount, "LsCount").Column("Ls_Count");
        }
    }
}
