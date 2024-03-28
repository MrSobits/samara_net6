/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Nso.Map
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
///             References(x => x.RealityObject, "REALITY_OBJECT_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Nso.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Nso.Entities.PaymentAccount"</summary>
    public class PaymentAccountMap : JoinedSubClassMap<PaymentAccount>
    {
        
        public PaymentAccountMap() : 
                base("Bars.Gkh.Overhaul.Nso.Entities.PaymentAccount", "OVRHL_PAY_ACCOUNT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.OverdraftLimit, "OverdraftLimit").Column("OVERDRAFT_LIMIT");
            Reference(x => x.AccountOwner, "AccountOwner").Column("OWNER_ID");
            Reference(x => x.RealityObject, "RealityObject").Column("REALITY_OBJECT_ID").NotNull().Fetch();
        }
    }
}
