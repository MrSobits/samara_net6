/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using B4.DataAccess;
///     using Bars.Gkh.Entities;
/// 
///     class FiasAddressUidMap : BaseImportableEntityMap<FiasAddressUid>
///     {
///         public FiasAddressUidMap()
///             : base("B4_FIAS_ADDRESS_UID")
///         {
///             Map(x => x.Uid, "ICS_UID");
/// 
///             Map(x => x.BillingId, "BILLING_ID");
/// 
///             References(x => x.Address, "FIAS_ADDRESS_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Entities.FiasAddressUid"</summary>
    public class FiasAddressUidMap : BaseImportableEntityMap<FiasAddressUid>
    {
        
        public FiasAddressUidMap() : 
                base("Bars.Gkh.Entities.FiasAddressUid", "B4_FIAS_ADDRESS_UID")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Uid, "Uid").Column("ICS_UID");
            Property(x => x.BillingId, "BillingId").Column("BILLING_ID");
            Reference(x => x.Address, "Address").Column("FIAS_ADDRESS_ID").Fetch();
        }
    }
}
