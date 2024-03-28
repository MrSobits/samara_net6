/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Nso.Map.LongTermProgram
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Nso.Entities;
/// 
///     public class RegOpAccountDecisionMap : BaseJoinedSubclassMap<RegOpAccountDecision>
///     {
///         public RegOpAccountDecisionMap()
///             : base("OVRHL_PR_DEC_REGOP_ACC", "ID")
///         {
///             //References(x => x.RegOperator, "REG_OPERATOR_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Nso.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Nso.Entities.RegOpAccountDecision"</summary>
    public class RegOpAccountDecisionMap : JoinedSubClassMap<RegOpAccountDecision>
    {
        
        public RegOpAccountDecisionMap() : 
                base("Bars.Gkh.Overhaul.Nso.Entities.RegOpAccountDecision", "OVRHL_PR_DEC_REGOP_ACC")
        {
        }
        
        protected override void Map()
        {
        }
    }
}
