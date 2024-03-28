namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    /// <summary>Маппинг для сущности "Задача проверки приказа ГЖИ"</summary>
	public class DisposalSurveyObjectiveMap : BaseEntityMap<DisposalSurveyObjective>
    {
        public DisposalSurveyObjectiveMap() : 
                base("Задача проверки приказа ГЖИ", "GJI_NSO_DISPOSAL_SURVEY_OBJ")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Disposal, "Распоряжение ГЖИ").Column("DISPOSAL_ID").NotNull().Fetch();
            Reference(x => x.SurveyObjective, "Задача проверки").Column("SURVEY_OBJ_ID").NotNull().Fetch();
            Property(x => x.Description, "Description").Column("DESCRIPTION");
        }
    }
} 
