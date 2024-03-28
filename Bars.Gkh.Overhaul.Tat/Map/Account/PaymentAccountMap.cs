/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Tat.Map
/// {
///     using Entities;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     public class PaymentAccountMap : SubclassMap<PaymentAccount>
///     {
///         public PaymentAccountMap()
///         {
///             Table("OVRHL_PAY_ACCOUNT");
///             KeyColumn("ID");
/// 
///             Map(x => x.OverdraftLimit, "OVERDRAFT_LIMIT");
///             References(x => x.AccountOwner, "OWNER_ID");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Tat.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Tat.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Tat.Entities.PaymentAccount"</summary>
    public class PaymentAccountMap : JoinedSubClassMap<PaymentAccount>
    {
        
        public PaymentAccountMap() : 
                base("Bars.Gkh.Overhaul.Tat.Entities.PaymentAccount", "OVRHL_PAY_ACCOUNT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.OverdraftLimit, "OverdraftLimit").Column("OVERDRAFT_LIMIT");
            Reference(x => x.AccountOwner, "AccountOwner").Column("OWNER_ID");
        }
    }
}
