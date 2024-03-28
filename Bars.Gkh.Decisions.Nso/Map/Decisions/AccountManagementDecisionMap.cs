/// <mapping-converter-backup>
/// namespace Bars.Gkh.Decisions.Nso.Map.Decisions
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Decisions.Nso.Entities.Decisions;
/// 
///     public class AccountManagementDecisionMap : BaseJoinedSubclassMap<AccountManagementDecision>
///     {
///         public AccountManagementDecisionMap()
///             : base("DEC_ACCOUNT_MANAGE", "ID")
///         {
///             Map(x => x.Decision, "DECISION");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Decisions.Nso.Map.Decisions
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Decisions.Nso.Entities.Decisions;
    
    
    /// <summary>Маппинг для "Решение по ведение лицевого счета"</summary>
    public class AccountManagementDecisionMap : JoinedSubClassMap<AccountManagementDecision>
    {
        
        public AccountManagementDecisionMap() : 
                base("Решение по ведение лицевого счета", "DEC_ACCOUNT_MANAGE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Decision, "Способ ведения лицевого счета").Column("DECISION");
        }
    }
}
