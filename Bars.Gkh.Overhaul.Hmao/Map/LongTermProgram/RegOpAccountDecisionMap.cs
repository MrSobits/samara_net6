/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Hmao.Map.LongTermProgram
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Hmao.Entities;
/// 
///     public class RegOpAccountDecisionMap : BaseJoinedSubclassMap<RegOpAccountDecision>
///     {
///         public RegOpAccountDecisionMap()
///             : base("OVRHL_PR_DEC_REGOP_ACC", "ID")
///         {
///            // References(x => x.RegOperator, "REG_OPERATOR_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Hmao.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    
    
    /// <summary>Маппинг для "Решение собственников помещений МКД (при формирования фонда КР на счете Рег.оператора)"</summary>
    public class RegOpAccountDecisionMap : JoinedSubClassMap<RegOpAccountDecision>
    {
        
        public RegOpAccountDecisionMap() : 
                base("Решение собственников помещений МКД (при формирования фонда КР на счете Рег.опера" +
                        "тора)", "OVRHL_PR_DEC_REGOP_ACC")
        {
        }
        
        protected override void Map()
        {
        }
    }
}
