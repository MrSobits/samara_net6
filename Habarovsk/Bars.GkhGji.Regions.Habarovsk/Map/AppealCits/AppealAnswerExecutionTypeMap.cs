namespace Bars.GkhGji.Regions.Habarovsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;
    
    
    public class AppealAnswerExecutionTypeMap : BaseEntityMap<AppealAnswerExecutionType>
    {
        
        public AppealAnswerExecutionTypeMap() : 
                base("Типы исполнения по ответу на обращения", "GJI_APPCIT_ANSWER_EXECUTION_TYPE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.AppealCitsAnswer, "AppealCitsAnswer").Column("APPCIT_ANSWER_ID").NotNull().Fetch();
            Reference(x => x.AppealExecutionType, "AppealExecutionType").Column("AE_TYPE_ID").NotNull().Fetch();
        }
    }
    
}
