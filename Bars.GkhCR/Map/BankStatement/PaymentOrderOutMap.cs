/// <mapping-converter-backup>
/// namespace Bars.B4.DataAccess
/// {
///     using Bars.GkhCr.Entities;
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Маппинг для сущности PaymentOrderOut
///     /// </summary>
///     public class PaymentOrderOutMap : SubclassMap<PaymentOrderOut>
///     {
///         public PaymentOrderOutMap()
///         {
///             Table("CR_PAYMENT_ORDER_OUT");
///             KeyColumn("ID");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Сущность платежного поручения расход"</summary>
    public class PaymentOrderOutMap : JoinedSubClassMap<PaymentOrderOut>
    {
        
        public PaymentOrderOutMap() : 
                base("Сущность платежного поручения расход", "CR_PAYMENT_ORDER_OUT")
        {
        }
        
        protected override void Map()
        {
        }
    }
}
