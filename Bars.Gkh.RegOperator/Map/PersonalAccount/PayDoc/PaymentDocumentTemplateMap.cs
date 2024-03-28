/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map.PersonalAccount.PayDoc
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
/// 
///     internal class PaymentDocumentTemplateMap : BaseEntityMap<PaymentDocumentTemplate>
///     {
///         public PaymentDocumentTemplateMap() : base("regop_payment_doc_templ")
///         {
///             Map(x => x.Template, "template_bytes");
///             Map(x => x.TemplateCode, "template_code");
/// 
///             References(x => x.Period, "period_id");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map.PersonalAccount.PayDoc
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
    
    /// <summary>Маппинг для "Шаблон документа на оплату по периоду"</summary>
    public class PaymentDocumentTemplateMap : BaseEntityMap<PaymentDocumentTemplate>
    {     
        public PaymentDocumentTemplateMap() : 
                base("Шаблон документа на оплату по периоду", "REGOP_PAYMENT_DOC_TEMPL")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.Period, "Период начисления").Column("PERIOD_ID");
            this.Property(x => x.TemplateCode, "Код шаблона").Column("TEMPLATE_CODE").Length(250);
            this.Property(x => x.Template, "Шаблон").Column("TEMPLATE_BYTES");
        }
    }
}