/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Tat.Map
/// {
///     using Entities;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     public class RealAccountMap : SubclassMap<RealAccount>
///     {
///         public RealAccountMap()
///         {
///             Table("OVRHL_REAL_ACCOUNT");
///             KeyColumn("ID");
/// 
///             Map(x => x.OverdraftLimit, "OVERDRAFT_LIMIT");
///             References(x => x.AccountOwner, "OWNER_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Tat.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Tat.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Tat.Entities.RealAccount"</summary>
    public class RealAccountMap : JoinedSubClassMap<RealAccount>
    {
        
        public RealAccountMap() : 
                base("Bars.Gkh.Overhaul.Tat.Entities.RealAccount", "OVRHL_REAL_ACCOUNT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.OverdraftLimit, "OverdraftLimit").Column("OVERDRAFT_LIMIT");
            Reference(x => x.AccountOwner, "AccountOwner").Column("OWNER_ID").Fetch();
        }
    }
}
