/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map.Decisions
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.RegOperator.Entities.Decisions;
/// 
///     public sealed class CoreDecisionMap : BaseImportableEntityMap<CoreDecision>
///     {
///         public CoreDecisionMap()
///             : base("DEC_CORE_DECISION")
///         {
///             Map(x => x.DecisionType, "DECISION_TYPE", true);
///             References(x => x.GovDecision, "GOV_DECISION_ID");
///             References(x => x.UltimateDecision, "ULTIMATE_DECISION_ID");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map.Decisions
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities.Decisions;
    
    
    /// <summary>Маппинг для "Протокол решения"</summary>
    public class CoreDecisionMap : BaseImportableEntityMap<CoreDecision>
    {
        
        public CoreDecisionMap() : 
                base("Протокол решения", "DEC_CORE_DECISION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DecisionType, "Тип протокола решения").Column("DECISION_TYPE").NotNull();
            Reference(x => x.GovDecision, "Протокол решения органа государственной власти").Column("GOV_DECISION_ID");
            Reference(x => x.UltimateDecision, "Протокол решения собственников.").Column("ULTIMATE_DECISION_ID");
        }
    }
}
