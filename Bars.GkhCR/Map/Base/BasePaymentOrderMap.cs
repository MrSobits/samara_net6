/// <mapping-converter-backup>
/// namespace Bars.B4.DataAccess
/// {
///     using Bars.Gkh.Map;;
///     using Bars.GkhCr.Entities;
///     using Bars.GkhCr.Enums;
/// 
///     /// <summary>
///     /// Базовый мап для BasePaymentOrder
///     /// </summary>
///     public class BasePaymentOrderMap : BaseGkhEntityMap<BasePaymentOrder>
///     {
///         public BasePaymentOrderMap() : base("CR_PAYMENT_ORDER")
///         {
///             Map(x => x.TypePaymentOrder, "TYPE_PAYMENT_ORDER").Not.Nullable().CustomType<TypePaymentOrder>();
///             Map(x => x.TypeFinanceSource, "TYPE_FIN_SOURCE").Not.Nullable().CustomType<TypeFinanceSource>();
///             Map(x => x.PayPurpose, "PAY_PURPOSE").Length(300);
///             Map(x => x.BidNum, "BID_NUM").Length(300);
///             Map(x => x.DocumentNum, "DOCUMENT_NUM").Length(300);
///             Map(x => x.BidDate, "BID_DATE");
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.Sum, "SUM");
///             Map(x => x.RedirectFunds, "REDIRECT_FUNDS");
///             Map(x => x.RepeatSend, "REPEAT_SEND").Not.Nullable();
///             Map(x => x.DocId, "IDDOC").Length(250);
/// 
///             References(x => x.BankStatement, "BANK_STATEMENT_ID").Fetch.Join();
///             //References(x => x.FinanceSource, "FIN_SOURCE_ID").Fetch.Join();
///             References(x => x.PayerContragent, "PAYER_CONTRAGENT_ID").Fetch.Join();
///             References(x => x.ReceiverContragent, "RECEIVER_CONTRAGENT_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Базовая сущность платежного поручения"</summary>
    public class BasePaymentOrderMap : BaseImportableEntityMap<BasePaymentOrder>
    {
        
        public BasePaymentOrderMap() : 
                base("Базовая сущность платежного поручения", "CR_PAYMENT_ORDER")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.TypePaymentOrder, "Тип платежного поручения").Column("TYPE_PAYMENT_ORDER").NotNull();
            Property(x => x.TypeFinanceSource, "Тип источника финансирования").Column("TYPE_FIN_SOURCE").NotNull();
            Property(x => x.PayPurpose, "Назначение платежа").Column("PAY_PURPOSE").Length(300);
            Property(x => x.BidNum, "Номер заявки").Column("BID_NUM").Length(300);
            Property(x => x.DocumentNum, "Номер").Column("DOCUMENT_NUM").Length(300);
            Property(x => x.BidDate, "Дата заявки").Column("BID_DATE");
            Property(x => x.DocumentDate, "Дата").Column("DOCUMENT_DATE");
            Property(x => x.Sum, "Сумма по документу").Column("SUM");
            Property(x => x.RedirectFunds, "сумма повторных направленных средств").Column("REDIRECT_FUNDS");
            Property(x => x.RepeatSend, "Повторно направленные средства").Column("REPEAT_SEND").NotNull();
            Property(x => x.DocId, "Id документа").Column("IDDOC").Length(250);
            Reference(x => x.BankStatement, "Банковская выписка").Column("BANK_STATEMENT_ID").Fetch();
            Reference(x => x.PayerContragent, "Плательщик").Column("PAYER_CONTRAGENT_ID").Fetch();
            Reference(x => x.ReceiverContragent, "Получатель").Column("RECEIVER_CONTRAGENT_ID").Fetch();
        }
    }
}
