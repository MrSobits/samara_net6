/// <mapping-converter-backup>
/// namespace Bars.Gkh.Decisions.Nso.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class AccumulationTransferDecisionMap : BaseJoinedSubclassMap<AccumulationTransferDecision>
///     {
///         public AccumulationTransferDecisionMap() : base("DEC_ACCUM_TRANSFER", "ID")
///         {
///             Map(x => x.Decision, "DECISION_VALUE", true);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Decisions.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Decisions.Nso.Entities;
    
    
    /// <summary>Маппинг для "Решение о переводе накоплений"</summary>
    public class AccumulationTransferDecisionMap : JoinedSubClassMap<AccumulationTransferDecision>
    {
        
        public AccumulationTransferDecisionMap() : 
                base("Решение о переводе накоплений", "DEC_ACCUM_TRANSFER")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Decision, "Сумма накоплений переводимая на спецсчет").Column("DECISION_VALUE").NotNull();
        }
    }
}
