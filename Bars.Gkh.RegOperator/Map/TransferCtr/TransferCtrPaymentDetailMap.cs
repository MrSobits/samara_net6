/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     /// <summary>
///     /// Маппинг сущности "Детализация оплат заявки на перечисление средств подрядчикам"
///     /// </summary>
///     public class TransferCtrPaymentDetailMap : BaseImportableEntityMap<TransferCtrPaymentDetail>
///     {
///         /// <summary>
///         /// .ctor
///         /// </summary>
///         public TransferCtrPaymentDetailMap() : base("REGOP_TRANSFER_CTR_DETAIL")
///         {
///             References(x => x.TransferCtr, "TRANSFER_CTR_ID");
///             References(x => x.Wallet, "WALLET_ID");
/// 
///             Map(x => x.PaidSum, "PAID_SUM");
///             Map(x => x.RefundSum, "REFUND_SUM");
///             Map(x => x.Amount, "AMOUNT");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Детализация оплат заявки на перечисление средств подрядчикам"</summary>
    public class TransferCtrPaymentDetailMap : BaseImportableEntityMap<TransferCtrPaymentDetail>
    {
        
        public TransferCtrPaymentDetailMap() : 
                base("Детализация оплат заявки на перечисление средств подрядчикам", "REGOP_TRANSFER_CTR_DETAIL")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.TransferCtr, "Заявка на перечисление средств подрядчикам").Column("TRANSFER_CTR_ID");
            Reference(x => x.Wallet, "Кошелек").Column("WALLET_ID");
            Property(x => x.Amount, "Сумма к оплате").Column("AMOUNT");
            Property(x => x.PaidSum, "Оплаченная сумма").Column("PAID_SUM");
            Property(x => x.RefundSum, "Сумма возврата").Column("REFUND_SUM");
        }
    }
}
