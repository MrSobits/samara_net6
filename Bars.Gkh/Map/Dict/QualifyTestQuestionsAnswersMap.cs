namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Инспектор"</summary>
    public class QualifyTestQuestionsAnswersMap : BaseEntityMap<QualifyTestQuestionsAnswers>
    {
        
        public QualifyTestQuestionsAnswersMap() : 
                base("Варианты ответов к экзамену", "GKH_DICT_QTEST_QUANSWERS")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Code, "Код").Column("CODE").Length(10).NotNull();
            Property(x => x.Answer, "Вариант ответа").Column("ANSWER").Length(5000);
            Property(x => x.IsCorrect, "Является правильным").Column("IS_CORRECT").NotNull();
            Reference(x => x.QualifyTestQuestions, "Актуальный").Column("QUESTION_ID").NotNull().Fetch(); ;
           
        }
    }
}
