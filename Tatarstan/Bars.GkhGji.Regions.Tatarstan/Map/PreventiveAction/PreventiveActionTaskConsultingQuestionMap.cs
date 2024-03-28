namespace Bars.GkhGji.Regions.Tatarstan.Map.PreventiveAction
{
    using B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

    public class PreventiveActionTaskConsultingQuestionMap : BaseEntityMap<PreventiveActionTaskConsultingQuestion>
    {
        /// <inheritdoc />
        public PreventiveActionTaskConsultingQuestionMap()
            : base(nameof(PreventiveActionTaskConsultingQuestion), "PREVENTIVE_ACTION_TASK_CONSULTING_QUESTION")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.Task, "Задание профилактического мероприятия").Column("TASK_ID");
            this.Property(x => x.Question, "Вопрос").Column("QUESTION");
            this.Property(x => x.Answer, "Ответ").Column("ANSWER");
            this.Property(x => x.ControlledPerson, "Подконтрольное лицо").Column("CONTROLLED_PERSON");
        }
    }
}