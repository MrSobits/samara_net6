/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map.PersonalAccount
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.RegOperator.Entities.PersonalAccount;
/// 
///     public class PaymentDocumentMap : BaseEntityMap<PaymentDocument>
///     {
///         public PaymentDocumentMap()
///             : base("REGOP_PAYMENT_DOC_PRINT")
///         {
///             Map(x => x.Year, "DOC_YEAR", true);
///             Map(x => x.Number, "DOC_NUMBER", true);
///             Map(x => x.AccountId, "ACCOUNT_ID", true);
///             Map(x => x.PeriodId, "PERIOD_ID", true);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map.PersonalAccount
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    
    
    /// <summary>Маппинг для "История созданных документов на оплату. Сущность нужна только для нумерации документов"</summary>
    public class PaymentDocumentMap : BaseEntityMap<PaymentDocument>
    {
        
        public PaymentDocumentMap() : 
                base("История созданных документов на оплату. Сущность нужна только для нумерации докум" +
                        "ентов", "REGOP_PAYMENT_DOC_PRINT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Year, "Год").Column("DOC_YEAR").NotNull();
            Property(x => x.Number, "Номер").Column("DOC_NUMBER").NotNull();
            Property(x => x.AccountId, "идентификатор лицевого счета").Column("ACCOUNT_ID").NotNull();
            Property(x => x.PeriodId, "идентификатор периода").Column("PERIOD_ID").NotNull();
        }
    }
}
