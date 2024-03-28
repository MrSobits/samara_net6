namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Инспектор"</summary>
    public class QualifyTestQuestionsMap : BaseEntityMap<QualifyTestQuestions>
    {
        
        public QualifyTestQuestionsMap() : 
                base("Вопросы к экзамену", "GKH_DICT_QTEST_QUESTIONS")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Code, "Код").Column("CODE").Length(10).NotNull();
            Property(x => x.Name, "Наименование").Column("NAME").Length(500);
            Property(x => x.Question, "Вопрос").Column("QUESTION_TEXT").Length(5000).NotNull();
            Property(x => x.IsActual, "Актуальный").Column("IS_ACTUAL").NotNull();
           
        }
    }
}
