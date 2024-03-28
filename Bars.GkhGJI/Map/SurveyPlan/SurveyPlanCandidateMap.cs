/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map.SurveyPlan
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.GkhGji.Entities.SurveyPlan;
/// 
///     public class SurveyPlanCandidateMap : BaseEntityMap<SurveyPlanCandidate>
///     {
///         public SurveyPlanCandidateMap()
///             : base("GJI_SURV_PLAN_CAND")
///         {
///             Map(x => x.PlanMonth, "PLAN_MONTH", true);
///             Map(x => x.PlanYear, "PLAN_YEAR", true);
///             Map(x => x.Reason, "REASON", true, 1000);
///             Map(x => x.FromLastAuditDate, "FROM_LAST_AUDIT", true, false);
/// 
///             References(x => x.AuditPurpose, "AUDIT_PURPOSE_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.Contragent, "CONTRAGENT_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map.SurveyPlan
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities.SurveyPlan;
    using System;
    
    
    /// <summary>Маппинг для "Предрассчитанный кандидат на добавление в план"</summary>
    public class SurveyPlanCandidateMap : BaseEntityMap<SurveyPlanCandidate>
    {
        
        public SurveyPlanCandidateMap() : 
                base("Предрассчитанный кандидат на добавление в план", "GJI_SURV_PLAN_CAND")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.AuditPurpose, "Цель проверки").Column("AUDIT_PURPOSE_ID").NotNull().Fetch();
            Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID").NotNull().Fetch();
            Property(x => x.FromLastAuditDate, "Признак расчета даты на основании предыдущей проверки").Column("FROM_LAST_AUDIT").DefaultValue(false).NotNull();
            Property(x => x.PlanMonth, "Рассчитанный плановый месяц").Column("PLAN_MONTH").NotNull();
            Property(x => x.PlanYear, "Рассчитанный плановый год").Column("PLAN_YEAR").NotNull();
            Property(x => x.Reason, "Причина включения в план").Column("REASON").Length(1000).NotNull();
        }
    }
}
