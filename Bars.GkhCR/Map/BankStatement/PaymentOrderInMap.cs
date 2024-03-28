/// <mapping-converter-backup>
/// namespace Bars.B4.DataAccess
/// {
///     using Bars.GkhCr.Entities;
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Маппинг для сущности PaymentOrderIn
///     /// </summary>
///     public class PaymentOrderInMap : SubclassMap<PaymentOrderIn>
///     {
///         public PaymentOrderInMap()
///         {
///             Table("CR_PAYMENT_ORDER_IN");
///             KeyColumn("ID");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Cущность платежного поручения приход"</summary>
    public class PaymentOrderInMap : JoinedSubClassMap<PaymentOrderIn>
    {
        
        public PaymentOrderInMap() : 
                base("Cущность платежного поручения приход", "CR_PAYMENT_ORDER_IN")
        {
        }
        
        protected override void Map()
        {
        }
    }
}
