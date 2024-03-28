/// <mapping-converter-backup>
/// namespace Bars.Gkh.Decisions.Nso.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class AccountOwnerDecisionMap : BaseJoinedSubclassMap<AccountOwnerDecision>
///     {
///         public AccountOwnerDecisionMap() : base("DEC_ACCOUNT_OWNER", "ID")
///         {
///             Map(x => x.DecisionType, "DECISION_TYPE");
/// 
///             //References(x => x.Decision, "CONTRAGENT_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Decisions.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Decisions.Nso.Entities;
    
    
    /// <summary>Маппинг для "Решение о владельце счета"</summary>
    public class AccountOwnerDecisionMap : JoinedSubClassMap<AccountOwnerDecision>
    {
        
        public AccountOwnerDecisionMap() : 
                base("Решение о владельце счета", "DEC_ACCOUNT_OWNER")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DecisionType, "Владелец счета").Column("DECISION_TYPE");
        }
    }
}
