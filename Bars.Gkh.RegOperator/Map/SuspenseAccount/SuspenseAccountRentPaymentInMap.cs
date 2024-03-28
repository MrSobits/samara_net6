/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using B4.DataAccess;
///     using Entities;
/// 
///     public class SuspenseAccountRentPaymentInMap : BaseImportableEntityMap<SuspenseAccountRentPaymentIn>
///     {
///         public SuspenseAccountRentPaymentInMap()
///             : base("REGOP_SUSPACC_RENTPAY")
///         {
///             References(x => x.Payment, "PAYMENT_ID").Not.Nullable().Fetch.Join();
///             References(x => x.SuspenceAccount, "SUSPACC_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.RegOperator.Entities.SuspenseAccountRentPaymentIn"</summary>
    public class SuspenseAccountRentPaymentInMap : BaseImportableEntityMap<SuspenseAccountRentPaymentIn>
    {
        
        public SuspenseAccountRentPaymentInMap() : 
                base("Bars.Gkh.RegOperator.Entities.SuspenseAccountRentPaymentIn", "REGOP_SUSPACC_RENTPAY")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Payment, "Payment").Column("PAYMENT_ID").NotNull().Fetch();
            Reference(x => x.SuspenceAccount, "SuspenceAccount").Column("SUSPACC_ID").NotNull().Fetch();
        }
    }
}
