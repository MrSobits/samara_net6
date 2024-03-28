/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.Gkh.RegOperator.Entities;
/// 
///     public class PaymentOrderDetailMap : BaseImportableEntityMap<PaymentOrderDetail>
///     {
///         public PaymentOrderDetailMap()
///             : base("REGOP_PAYMENT_ORDER_DETAIL")
///         {
///             Map(x => x.PaidSum, "PAID_SUM");
///             Map(x => x.Amount, "AMOUNT");
/// 
///             References(x => x.PaymentOrder, "PAYMENT_ORDER_ID");
///             References(x => x.Wallet, "WALLET_ID");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Детализация распоряжения об оплате акта выполненных работ"</summary>
    public class PaymentOrderDetailMap : BaseImportableEntityMap<PaymentOrderDetail>
    {
        
        public PaymentOrderDetailMap() : 
                base("Детализация распоряжения об оплате акта выполненных работ", "REGOP_PAYMENT_ORDER_DETAIL")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.PaidSum, "Оплаченная сумма").Column("PAID_SUM");
            Property(x => x.Amount, "Сумма к оплате, руб. (сколько денег взять из кошелька)").Column("AMOUNT");
            Reference(x => x.PaymentOrder, "Распоряжение по оплате").Column("PAYMENT_ORDER_ID");
            Reference(x => x.Wallet, "Кошелек, с которого надо вять деньги при оплате (Источник финансирования)").Column("WALLET_ID");
        }
    }
}
