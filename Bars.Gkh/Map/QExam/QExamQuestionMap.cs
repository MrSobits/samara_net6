namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Инспектор"</summary>
    public class QExamQuestionMap : BaseImportableEntityMap<QExamQuestion>
    {
        
        public QExamQuestionMap() : 
                base("Вопросы к экзамену", "GKH_QEXAM_QUESTION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Number, "Код").Column("NUMBER").Length(10);
            Property(x => x.QuestionText, "Текст вопроса").Column("QUESTION_TEXT").Length(5000);
            Reference(x => x.QualifyTestQuestions, "Вопрос").Column("QUESTION_ID").NotNull().Fetch();
            Reference(x => x.QualifyTestQuestionsAnswers, "Выбранный ответ").Column("ANSWER_ID").Fetch();
            Reference(x => x.PersonRequestToExam, "Запрос на экзамен").Column("REQUEST_ID").NotNull().Fetch();

        }
    }
}
