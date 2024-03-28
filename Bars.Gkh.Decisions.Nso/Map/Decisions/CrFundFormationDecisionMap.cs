/// <mapping-converter-backup>
/// namespace Bars.Gkh.Decisions.Nso.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class CrFundFormationDecisionMap : BaseJoinedSubclassMap<CrFundFormationDecision>
///     {
///         public CrFundFormationDecisionMap() : base("DEC_CR_FUND", "ID")
///         {
///             Map(x => x.Decision, "DECISION_VALUE");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Decisions.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Decisions.Nso.Entities;
    
    
    /// <summary>Маппинг для "Решение о формировании фонда КР"</summary>
    public class CrFundFormationDecisionMap : JoinedSubClassMap<CrFundFormationDecision>
    {
        
        public CrFundFormationDecisionMap() : 
                base("Решение о формировании фонда КР", "DEC_CR_FUND")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Decision, "Тип формирования фонда КР").Column("DECISION_VALUE");
        }
    }
}
