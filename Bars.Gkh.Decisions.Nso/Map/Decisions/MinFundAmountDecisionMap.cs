/// <mapping-converter-backup>
/// namespace Bars.Gkh.Decisions.Nso.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class MinFundAmountDecisionMap : BaseJoinedSubclassMap<MinFundAmountDecision>
///     {
///         public MinFundAmountDecisionMap() : base("DEC_MIN_FUND_AMOUNT", "ID")
///         {
///             Map(x => x.Decision, "DECISION_VALUE");
///             Map(x => x.Default, "DEFAULT_VALUE");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Decisions.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Decisions.Nso.Entities;
    
    
    /// <summary>Маппинг для "Размер минимального фонда на КР"</summary>
    public class MinFundAmountDecisionMap : JoinedSubClassMap<MinFundAmountDecision>
    {
        
        public MinFundAmountDecisionMap() : 
                base("Размер минимального фонда на КР", "DEC_MIN_FUND_AMOUNT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Decision, "Текущее значение").Column("DECISION_VALUE");
            Property(x => x.Default, "Минимальное значение").Column("DEFAULT_VALUE");
        }
    }
}
