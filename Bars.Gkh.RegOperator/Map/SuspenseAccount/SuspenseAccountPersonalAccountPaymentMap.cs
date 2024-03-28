/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.RegOperator.Entities;
/// 
///     public class SuspenseAccountPersonalAccountPaymentMap : BaseImportableEntityMap<SuspenseAccountPersonalAccountPayment>
///     {
///         public SuspenseAccountPersonalAccountPaymentMap()
///             : base("REG_OP_SSP_ACC_PA_P")
///         {
///             References(x => x.Payment, "PAY_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.SuspenseAccount, "SA_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Связка записи из счета невясненных сумм и оплатой по ЛС"</summary>
    public class SuspenseAccountPersonalAccountPaymentMap : BaseImportableEntityMap<SuspenseAccountPersonalAccountPayment>
    {
        
        public SuspenseAccountPersonalAccountPaymentMap() : 
                base("Связка записи из счета невясненных сумм и оплатой по ЛС", "REG_OP_SSP_ACC_PA_P")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.SuspenseAccount, "SuspenseAccount").Column("SA_ID").NotNull().Fetch();
            Reference(x => x.Payment, "Payment").Column("PAY_ID").NotNull().Fetch();
        }
    }
}
