/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Nso.Map
/// {
///     using Entities;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     public class SpecialAccountMap: SubclassMap<SpecialAccount>
///     {
///         public SpecialAccountMap()
///         {
///             Table("OVRHL_SPECIAL_ACCOUNT");
///             KeyColumn("ID");
/// 
///             References(x => x.AccountOwner, "OWNER_ID").Fetch.Join();
///             References(x => x.CreditOrganization, "CREDIT_ORG_ID").Fetch.Join();
///             References(x => x.Decision, "DECISION_ID").Fetch.Join();
///             References(x => x.RealityObject, "REALITY_OBJECT_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Nso.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Nso.Entities.SpecialAccount"</summary>
    public class SpecialAccountMap : JoinedSubClassMap<SpecialAccount>
    {
        
        public SpecialAccountMap() : 
                base("Bars.Gkh.Overhaul.Nso.Entities.SpecialAccount", "OVRHL_SPECIAL_ACCOUNT")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.AccountOwner, "AccountOwner").Column("OWNER_ID").Fetch();
            Reference(x => x.CreditOrganization, "CreditOrganization").Column("CREDIT_ORG_ID").Fetch();
            Reference(x => x.Decision, "Decision").Column("DECISION_ID").Fetch();
            Reference(x => x.RealityObject, "RealityObject").Column("REALITY_OBJECT_ID").NotNull().Fetch();
        }
    }
}
