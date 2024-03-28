
namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Дата и время проведения проверки"</summary>
    public class ActCheckControlListAnswerMap : BaseEntityMap<ActCheckControlListAnswer>
    {
        
        public ActCheckControlListAnswerMap() : 
                base("Дата и время проведения проверки", "GJI_ACTCHECK_CONTROLLIST_ANSWER")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Question, "Вопрос").Column("QUESTION");
            Property(x => x.Description, "Примечание").Column("DESCRIPTION");
            Property(x => x.NpdName, "Дата окончания").Column("NPD");
            Property(x => x.YesNoNotApplicable, "Дата окончания").Column("ANSWER");
            Reference(x => x.ControlListQuestion, "Вопрос контрольного листа").Column("QUESTION_ID").NotNull().Fetch();
            Reference(x => x.ActCheck, "Акт проверки").Column("ACTCHECK_ID").NotNull().Fetch();
        }
    }
}
