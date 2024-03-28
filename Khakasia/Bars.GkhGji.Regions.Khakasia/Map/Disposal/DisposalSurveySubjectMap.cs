namespace Bars.GkhGji.Regions.Khakasia.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Khakasia.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Khakasia.Entities.DisposalSurveySubject"</summary>
    public class DisposalSurveySubjectMap : BaseEntityMap<DisposalSurveySubject>
    {
        
        public DisposalSurveySubjectMap() : 
                base("Bars.GkhGji.Regions.Khakasia.Entities.DisposalSurveySubject", "GJI_DISP_SURVSUBJ")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Disposal, "Disposal").Column("DISPOSAL_ID").NotNull().Fetch();
            Reference(x => x.SurveySubject, "SurveySubject").Column("SURVEY_SUBJ_ID").NotNull().Fetch();
        }
    }
}
