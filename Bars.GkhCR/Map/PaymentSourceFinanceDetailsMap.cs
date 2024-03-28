/// <mapping-converter-backup>
/// namespace Bars.GkhCr.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.GkhCr.Entities;
/// 
///     public class PaymentSrcFinanceDetailsMap : BaseImportableEntityMap<PaymentSrcFinanceDetails>
///     {
///         public PaymentSrcFinanceDetailsMap()
///             : base("CR_ACTPAYMENT_DETAILS")
///         {
///             Map(x => x.Balance, "BALANCE");
///             Map(x => x.Payment, "PAYMENT");
///             Map(x => x.SrcFinanceType, "SRC_FIN_TYPE", true);
/// 
///             References(x => x.ActPayment, "ACTPAYMENT_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Детализация по источнику финансирвоания оплаты акта выполненных работ"</summary>
    public class PaymentSrcFinanceDetailsMap : BaseImportableEntityMap<PaymentSrcFinanceDetails>
    {
        
        public PaymentSrcFinanceDetailsMap() : 
                base("Детализация по источнику финансирвоания оплаты акта выполненных работ", "CR_ACTPAYMENT_DETAILS")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.ActPayment, "ссылка на оплату акта выполненных работ").Column("ACTPAYMENT_ID").Fetch();
            Property(x => x.SrcFinanceType, "Тип источника финансирвоания").Column("SRC_FIN_TYPE").NotNull();
            Property(x => x.Balance, "Сальдо").Column("BALANCE");
            Property(x => x.Payment, "Оплата").Column("PAYMENT");
        }
    }
}
