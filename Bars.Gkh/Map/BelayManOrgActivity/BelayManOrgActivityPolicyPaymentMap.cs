/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     /// <summary>
///     /// Маппинг сущности "Оплата договора"
///     /// </summary>
///     public class BelayManOrgActivityPolicyPaymentMap : BaseGkhEntityMap<Entities.BelayPolicyPayment>
///     {
///         public BelayManOrgActivityPolicyPaymentMap() : base("GKH_BELAY_POLICY_PAYMENT")
///         {
///             Map(x => x.PaymentDate, "PAYMENT_DATE");
///             Map(x => x.Name, "NAME").Length(300);
///             Map(x => x.DocumentNumber, "DOCUMENT_NUM").Length(300);
///             Map(x => x.Sum, "SUM");
/// 
///             References(x => x.BelayPolicy, "BELAY_POLICY_ID").Not.Nullable().Fetch.Join();
///             References(x => x.FileInfo, "FILE_INFO_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Оплата договора"</summary>
    public class BelayPolicyPaymentMap : BaseImportableEntityMap<BelayPolicyPayment>
    {
        
        public BelayPolicyPaymentMap() : 
                base("Оплата договора", "GKH_BELAY_POLICY_PAYMENT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.PaymentDate, "Дата оплаты").Column("PAYMENT_DATE");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300);
            Property(x => x.DocumentNumber, "Номер документа").Column("DOCUMENT_NUM").Length(300);
            Property(x => x.Sum, "Сумма, руб.").Column("SUM");
            Reference(x => x.BelayPolicy, "Страховой полис").Column("BELAY_POLICY_ID").NotNull().Fetch();
            Reference(x => x.FileInfo, "Файл").Column("FILE_INFO_ID").Fetch();
        }
    }
}
