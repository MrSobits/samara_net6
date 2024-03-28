namespace Bars.GkhGji.Regions.Khakasia.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Khakasia.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Khakasia.Entities.ActSurveyConclusion"</summary>
    public class ActSurveyConclusionMap : BaseEntityMap<ActSurveyConclusion>
    {
        
        public ActSurveyConclusionMap() : 
                base("Bars.GkhGji.Regions.Khakasia.Entities.ActSurveyConclusion", "GJI_ACTSURVEY_CONCLUSION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DocNumber, "DocNumber").Column("DOC_NUMBER").Length(50);
            Property(x => x.DocDate, "DocDate").Column("DOC_DATE");
            Property(x => x.Description, "Description").Column("DESCRIPTION").Length(2000);
            Reference(x => x.Official, "Official").Column("OFFICIAL_ID").Fetch();
            Reference(x => x.File, "File").Column("FILE_ID").Fetch();
            Reference(x => x.ActSurvey, "ActSurvey").Column("ACT_SURVEY_ID").NotNull().Fetch();
        }
    }
}
