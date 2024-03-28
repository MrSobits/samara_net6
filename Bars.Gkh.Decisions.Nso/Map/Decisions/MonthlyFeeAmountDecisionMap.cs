/// <mapping-converter-backup>
/// namespace Bars.Gkh.Decisions.Nso.Map
/// {
///     using System.Collections.Generic;
///     using B4.DataAccess.ByCode;
///     using B4.DataAccess.UserTypes;
///     using Entities;
/// 
///     public class MonthlyFeeAmountDecisionMap : BaseJoinedSubclassMap<MonthlyFeeAmountDecision>
///     {
///         public MonthlyFeeAmountDecisionMap() : base("DEC_MONTHLY_FEE", "ID")
///         {
///             Property(x => x.Decision, m =>
///             {
///                 m.Column("DECISION_VALUE");
///                 m.NotNullable(false);
///                 m.Type<ImprovedJsonSerializedType<List<PeriodMonthlyFee>>>();
///             });
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Decisions.Nso.Map
{
    using System.Collections.Generic;
    using B4.Modules.Mapping.Mappers;

    using Bars.Gkh.DataAccess;
    using Bars.Gkh.Decisions.Nso.Entities;
    using NHibernate.Mapping.ByCode.Conformist;


    /// <summary>Маппинг для "Размер ежемесячного взноса на КР"</summary>
    public class MonthlyFeeAmountDecisionMap : JoinedSubClassMap<MonthlyFeeAmountDecision>
    {
        
        public MonthlyFeeAmountDecisionMap() : 
                base("Размер ежемесячного взноса на КР", "DEC_MONTHLY_FEE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Decision, "Текущие взносы").Column("DECISION_VALUE");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class MonthlyFeeAmountDecisionNhibernateMap : JoinedSubclassMapping<MonthlyFeeAmountDecision>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public MonthlyFeeAmountDecisionNhibernateMap()
        {
            Property(
                x => x.Decision,
                m =>
                    {
                        m.Type<ImprovedJsonSerializedType<List<PeriodMonthlyFee>>>();
                    });
        }
    }
}