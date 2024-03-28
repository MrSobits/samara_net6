/// <mapping-converter-backup>
/// namespace Bars.Gkh.Decisions.Nso.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class MkdManagementDecisionMap : BaseJoinedSubclassMap<MkdManagementDecision>
///     {
///         public MkdManagementDecisionMap() : base("DEC_MKD_MANAGE", "ID")
///         {
///             Map(x => x.DecisionType, "DECISION_TYPE");
/// 
///             References(x => x.Decision, "MANORG_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Decisions.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Decisions.Nso.Entities;
    
    
    /// <summary>Маппинг для "Решение о выборе управления"</summary>
    public class MkdManagementDecisionMap : JoinedSubClassMap<MkdManagementDecision>
    {
        
        public MkdManagementDecisionMap() : 
                base("Решение о выборе управления", "DEC_MKD_MANAGE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DecisionType, "Тип управления").Column("DECISION_TYPE");
            Reference(x => x.Decision, "Управляющая организация").Column("MANORG_ID").Fetch();
        }
    }
}
