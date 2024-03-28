/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Map
/// {
///     using Bars.Gkh.Overhaul.Entities;
/// 
///     public class PaymentSizeMuRecordMap : B4.DataAccess.BaseImportableEntityMap<PaymentSizeMuRecord>
///     {
///         public PaymentSizeMuRecordMap()
///             : base("OVRHL_PAYSIZE_MU_RECORD")
///         {
///             References(x => x.Municipality, "MUNICIPALITY_ID").Not.Nullable().Fetch.Join();
///             References(x => x.PaymentSizeCr, "PAYSIZECR_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Entities;
    
    
    /// <summary>Маппинг для "Сущность записи взноса на КР по муниципальному образованию"</summary>
    public class PaymentSizeMuRecordMap : BaseImportableEntityMap<PaymentSizeMuRecord>
    {
        
        public PaymentSizeMuRecordMap() : 
                base("Сущность записи взноса на КР по муниципальному образованию", "OVRHL_PAYSIZE_MU_RECORD")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Municipality, "Муниципальное образование").Column("MUNICIPALITY_ID").NotNull().Fetch();
            Reference(x => x.PaymentSizeCr, "Размер взноса на КР").Column("PAYSIZECR_ID").NotNull().Fetch();
        }
    }
}
