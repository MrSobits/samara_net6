namespace Bars.GkhGji.Regions.Tatarstan.Map.ActionIsolated
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;

    public class TaskActionIsolatedSurveyPurposeMap: BaseEntityMap<TaskActionIsolatedSurveyPurpose>
    {
        public TaskActionIsolatedSurveyPurposeMap()
            : base(nameof(TaskActionIsolatedSurveyPurpose), "GJI_TASK_ACTIONISOLATED_SURVEY_PURP")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.TaskActionIsolated, "Задание КНМ").Column("TASK_ACTIONISOLATED_ID").NotNull().Fetch();
            Reference(x => x.SurveyPurpose, "Цель проверки").Column("SURVEY_PURP_ID").NotNull().Fetch();
        }
    }
}