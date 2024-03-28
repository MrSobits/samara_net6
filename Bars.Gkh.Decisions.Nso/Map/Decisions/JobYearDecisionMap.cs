/// <mapping-converter-backup>
/// namespace Bars.Gkh.Decisions.Nso.Map
/// {
///     using System.Collections.Generic;
///     using B4.DataAccess.ByCode;
///     using B4.DataAccess.UserTypes;
///     using Entities;
/// 
///     public class JobYearDecisionMap : BaseJoinedSubclassMap<JobYearDecision>
///     {
///         public JobYearDecisionMap() : base("DEC_JOB_YEAR", "ID")
///         {
///             Property(x => x.JobYears, m =>
///             {
///                 m.Column("JOB_YEARS");
///                 m.NotNullable(false);
///                 m.Type<ImprovedJsonSerializedType<List<RealtyJobYear>>>();
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

    /// <summary>Маппинг для "Bars.Gkh.Decisions.Nso.Entities.JobYearDecision"</summary>
    public class JobYearDecisionMap : JoinedSubClassMap<JobYearDecision>
    {
        
        public JobYearDecisionMap() : 
                base("Bars.Gkh.Decisions.Nso.Entities.JobYearDecision", "DEC_JOB_YEAR")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.JobYears, "JobYears").Column("JOB_YEARS");
        }
    }

    public class JobYearDecisionNHibernateMapping : JoinedSubclassMapping<JobYearDecision>
    {
        public JobYearDecisionNHibernateMapping()
        {
            Property(x => x.JobYears, m =>
            {
                m.Type<ImprovedJsonSerializedType<List<RealtyJobYear>>>();
            });
        }
    }
}
