namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Инспектор"</summary>
    public class QExamQuestionAnswerMap : BaseImportableEntityMap<QExamQuestionAnswer>
    {
        
        public QExamQuestionAnswerMap() : 
                base("Варианты ответов к экзамену", "GKH_QEXAM_ANSWER")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.AnswerText, "Текст ответа").Column("ANSWER_TEXT").Length(5000);
            Reference(x => x.QExamQuestion, "Вопрос экзамена").Column("QUESTION_ID").NotNull().Fetch();
            Reference(x => x.QualifyTestQuestionsAnswers, "Ответ").Column("ANSWER_ID").NotNull().Fetch();
           
        }
    }
}
