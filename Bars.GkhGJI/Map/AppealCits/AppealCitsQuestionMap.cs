namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    public class AppealCitsQuestionMap : BaseEntityMap<AppealCitsQuestion>
    {
        public AppealCitsQuestionMap()
            : base("Виды вопросов обращения", "GJI_APPEAL_CITIZEN_QUESTIONS")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.AppealCits, "Обращение граждан").Column("CITIZENS_ID");
            this.Reference(x => x.QuestionKind, "Вид вопроса").Column("KIND_ID");
        }
    }
}
