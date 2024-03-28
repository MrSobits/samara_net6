/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map.SurveyPlan
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.GkhGji.Entities.SurveyPlan;
/// 
///     public class SurveyPlanContragentMap : BaseEntityMap<SurveyPlanContragent>
///     {
///         public SurveyPlanContragentMap()
///             : base("GJI_SURV_PLAN_CONTR")
///         {
///             Map(x => x.PlanMonth, "PLAN_MONTH", true);
///             Map(x => x.PlanYear, "PLAN_YEAR", true);
///             Map(x => x.Reason, "REASON", true, 1000);
/// 
///             Map(x => x.IsExcluded, "IS_EXCLUDED", true, false);
///             Map(x => x.ExclusionReason, "EXCLUSION_REASON", false, 2000);
/// 
///             Map(x => x.FromLastAuditDate, "FROM_LAST_AUDIT", true, false);
/// 
///             References(x => x.AuditPurpose, "AUDIT_PURPOSE_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.Contragent, "CONTRAGENT_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.SurveyPlan, "SURVEY_PLAN_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.Inspection, "INSPECTION_ID");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map.SurveyPlan
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities.SurveyPlan;
    using System;
    
    
    /// <summary>Маппинг для "Контрагент плана проверки"</summary>
    public class SurveyPlanContragentMap : BaseEntityMap<SurveyPlanContragent>
    {
        
        public SurveyPlanContragentMap() : 
                base("Контрагент плана проверки", "GJI_SURV_PLAN_CONTR")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.AuditPurpose, "Цель проверки").Column("AUDIT_PURPOSE_ID").NotNull().Fetch();
            Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID").NotNull().Fetch();
            Property(x => x.ExclusionReason, "Причина исключения").Column("EXCLUSION_REASON").Length(2000);
            Property(x => x.FromLastAuditDate, "Признак расчета даты на основании предыдущей проверки").Column("FROM_LAST_AUDIT").DefaultValue(false).NotNull();
            Reference(x => x.Inspection, "Сформированная проверка").Column("INSPECTION_ID");
            Property(x => x.IsExcluded, "Признак исключеия из плана").Column("IS_EXCLUDED").DefaultValue(false).NotNull();
            Property(x => x.PlanMonth, "Плановый месяц проверки").Column("PLAN_MONTH").NotNull();
            Property(x => x.PlanYear, "Плановый год проверки").Column("PLAN_YEAR").NotNull();
            Property(x => x.Reason, "Причина включения в план").Column("REASON").Length(1000).NotNull();
            Reference(x => x.SurveyPlan, "План проверки").Column("SURVEY_PLAN_ID").NotNull().Fetch();
        }
    }
}
