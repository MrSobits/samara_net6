/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map.SurveyPlan
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.GkhGji.Entities.SurveyPlan;
/// 
///     public class SurveyPlanMap : BaseEntityMap<SurveyPlan>
///     {
///         public SurveyPlanMap()
///             : base("GJI_SURVEY_PLAN")
///         {
///             Map(x => x.Name, "NAME", true, 500);
///             References(x => x.PlanJurPerson, "PLAN_JUR_PESON_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.State, "STATE_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map.SurveyPlan
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities.SurveyPlan;
    
    
    /// <summary>Маппинг для "План проверки"</summary>
    public class SurveyPlanMap : BaseEntityMap<SurveyPlan>
    {
        
        public SurveyPlanMap() : 
                base("План проверки", "GJI_SURVEY_PLAN")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Name, "Наименование плана").Column("NAME").Length(500).NotNull();
            Reference(x => x.PlanJurPerson, "План проверок юр лиц").Column("PLAN_JUR_PESON_ID").NotNull().Fetch();
            Reference(x => x.State, "Статус").Column("STATE_ID").NotNull().Fetch();
        }
    }
}
