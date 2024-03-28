namespace Bars.GkhGji.Regions.Tatarstan.Map.ActCheckAction
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction.SurveyAction;

    public class SurveyActionQuestionMap : BaseEntityMap<SurveyActionQuestion>
    {
        public SurveyActionQuestionMap()
            : base("Вопрос действия акта проверки с типом \"Опрос\"", "GJI_ACTCHECK_SURVEY_ACTION_QUESTION")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.SurveyAction, "Действие \"Опрос\"").Column("SURVEY_ACTION_ID");
            this.Property(x => x.Question, "Вопрос").Column("QUESTION");
            this.Property(x => x.Answer, "Ответ").Column("ANSWER");
        }
    }
}