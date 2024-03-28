namespace Bars.GkhGji.Map.Dict
{
	using Bars.B4.Modules.Mapping.Mappers;
	using Bars.GkhGji.Entities.Dict;

	/// <summary>Маппинг для "Bars.GkhGji.Map.Dict.SurveySubjectRequirement"</summary>
	public class SurveySubjectRequirementMap : BaseEntityMap<SurveySubjectRequirement>
    {
        
        public SurveySubjectRequirementMap() : 
                base("Bars.GkhGji.Map.Dict.SurveySubjectRequirement", "GJI_DICT_SURVEY_SUBJ_REQ")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.Code, "Code").Column("CODE").Length(300);
            this.Property(x => x.Name, "Name").Column("NAME").Length(500).NotNull();
        }
    }
}
