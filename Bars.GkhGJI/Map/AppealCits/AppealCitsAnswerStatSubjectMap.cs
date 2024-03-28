namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    public class AppealCitsAnswerStatSubjectMap : BaseEntityMap<AppealCitsAnswerStatSubject>
    {
        
        public AppealCitsAnswerStatSubjectMap() : 
                base("Таблица связи тематики, подтематики и характеристики", "GJI_APPCIT_ANSWER_STATSUBJ")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.AppealCitsAnswer, "Ответ").Column("ANSWER_ID").NotNull().Fetch();
            this.Reference(x => x.StatSubject, "Тематика обращения").Column("APPCIT_STAT_SUBJ_ID").NotNull().Fetch();
        }
    }
}
