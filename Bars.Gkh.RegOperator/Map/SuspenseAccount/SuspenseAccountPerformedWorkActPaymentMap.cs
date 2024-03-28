/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class SuspenseAccountPerformedWorkActPaymentMap : BaseImportableEntityMap<SuspenseAccountPerformedWorkActPayment>
///     {
///         public SuspenseAccountPerformedWorkActPaymentMap() : base("REGOP_SUSPACC_ACTPAYMENT")
///         {
///             References(x => x.PerformedWorkActPayment, "ACTPAY_ID", ReferenceMapConfig.NotNullAndFetchAndCascadeDelete);
///             References(x => x.SuspenseAccount, "SUSPACC_ID", ReferenceMapConfig.NotNullAndFetchAndCascadeDelete);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Связь записи счета НВС с оплатой акта"</summary>
    public class SuspenseAccountPerformedWorkActPaymentMap : BaseImportableEntityMap<SuspenseAccountPerformedWorkActPayment>
    {
        
        public SuspenseAccountPerformedWorkActPaymentMap() : 
                base("Связь записи счета НВС с оплатой акта", "REGOP_SUSPACC_ACTPAYMENT")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.SuspenseAccount, "SuspenseAccount").Column("SUSPACC_ID").NotNull().Fetch();
            Reference(x => x.PerformedWorkActPayment, "PerformedWorkActPayment").Column("ACTPAY_ID").NotNull().Fetch();
        }
    }
}
