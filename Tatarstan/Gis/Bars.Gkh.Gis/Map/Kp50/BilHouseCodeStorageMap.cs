/// <mapping-converter-backup>
/// using Bars.B4.DataAccess.ByCode;
/// using Bars.Gkh.Gis.Entities.Kp50;
/// 
/// namespace Bars.Gkh.Gis.Map.Kp50
/// {
///     public class BilHouseCodeStorageMap : PersistentObjectMap<BilHouseCodeStorage>
///     {
///         public  BilHouseCodeStorageMap()
///             : base("BIL_HOUSE_CODE_STORAGE")
///         {
///             Map(x => x.BillingHouseCode, "BILLING_HOUSE_CODE");
/// 
///             References(x => x.Schema, "SCHEMA_PREFIX_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Gis.Map.Kp50
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Gis.Entities.Kp50;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Gis.Entities.Kp50.BilHouseCodeStorage"</summary>
    public class BilHouseCodeStorageMap : PersistentObjectMap<BilHouseCodeStorage>
    {
        
        public BilHouseCodeStorageMap() : 
                base("Bars.Gkh.Gis.Entities.Kp50.BilHouseCodeStorage", "BIL_HOUSE_CODE_STORAGE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.BillingHouseCode, "BillingHouseCode").Column("BILLING_HOUSE_CODE");
            Reference(x => x.Schema, "Schema").Column("SCHEMA_PREFIX_ID").NotNull().Fetch();
        }
    }
}
