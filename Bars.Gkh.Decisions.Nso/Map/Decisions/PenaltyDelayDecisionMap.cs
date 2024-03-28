/// <mapping-converter-backup>
/// namespace Bars.Gkh.Decisions.Nso.Map.Decisions
/// {
///     using System.Collections.Generic;
///     using B4.DataAccess.ByCode;
///     using B4.DataAccess.UserTypes;
///     using Entities;
/// 
///     /// <summary>
///     /// 
///     /// </summary>
///     public class PenaltyDelayDecisionMap : BaseJoinedSubclassMap<PenaltyDelayDecision>
///     {
///         /// <summary>
///         /// .ctor
///         /// </summary>
///         public PenaltyDelayDecisionMap() : base("DEC_PENALTY_DELAY", "ID")
///         {
///             Property(x => x.Decision, m =>
///             {
///                 m.Column("DECISION");
///                 m.NotNullable(false);
///                 m.Type<ImprovedJsonSerializedType<List<OwnerPenaltyDelay>>>();
///             });
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Decisions.Nso.Map
{
    using System.Collections.Generic;
    
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.DataAccess;
    using Bars.Gkh.Decisions.Nso.Entities;

    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Решение о сроке уплаты взносов"</summary>
    public class PenaltyDelayDecisionMap : JoinedSubClassMap<PenaltyDelayDecision>
    {
        
        public PenaltyDelayDecisionMap() : 
                base("Решение о сроке уплаты взносов", "DEC_PENALTY_DELAY")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Decision, "Приянтое решение").Column("DECISION");
        }
    }

    public class PenaltyDelayDecisionNHibernateMapping : JoinedSubclassMapping<PenaltyDelayDecision>
    {
        public PenaltyDelayDecisionNHibernateMapping()
        {
            Property(
                x => x.Decision,
                m =>
                    {
                        m.Type<ImprovedJsonSerializedType<List<OwnerPenaltyDelay>>>();
                    });
        }
    }
}
