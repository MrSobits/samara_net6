/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.Gkh.RegOperator.Entities;
/// 
///     public class SuspenseAccountPreviousWorkPaymentMap : BaseImportableEntityMap<SuspenseAccountPreviousWorkPayment>
///     {
///         public SuspenseAccountPreviousWorkPaymentMap() : base("REGOP_SUSPACC_WORKPAY")
///         {
/// 
///             References(x => x.PreviousWorkPayment, "WORKPAY_ID").Not.Nullable().Fetch.Join();
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
    
    
    /// <summary>Маппинг для "Bars.Gkh.RegOperator.Entities.SuspenseAccountPreviousWorkPayment"</summary>
    public class SuspenseAccountPreviousWorkPaymentMap : BaseImportableEntityMap<SuspenseAccountPreviousWorkPayment>
    {
        
        public SuspenseAccountPreviousWorkPaymentMap() : 
                base("Bars.Gkh.RegOperator.Entities.SuspenseAccountPreviousWorkPayment", "REGOP_SUSPACC_WORKPAY")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.PreviousWorkPayment, "PreviousWorkPayment").Column("WORKPAY_ID").NotNull().Fetch();
            Reference(x => x.SuspenceAccount, "SuspenceAccount").Column("SUSPACC_ID").NotNull().Fetch();
        }
    }
}
