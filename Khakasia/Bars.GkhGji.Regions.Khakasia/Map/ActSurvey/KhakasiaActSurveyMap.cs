namespace Bars.GkhGji.Regions.Khakasia.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Khakasia.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Khakasia.Entities.KhakasiaActSurvey"</summary>
    public class KhakasiaActSurveyMap : JoinedSubClassMap<KhakasiaActSurvey>
    {
        
        public KhakasiaActSurveyMap() : 
                base("Bars.GkhGji.Regions.Khakasia.Entities.KhakasiaActSurvey", "KHAKASIA_GJI_ACTSURVEY")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ConclusionIssued, "ConclusionIssued").Column("CON_ISSUED").NotNull();
            Property(x => x.DateOf, "DateOf").Column("DATE_OF");
            Property(x => x.TimeEnd, "TimeEnd").Column("TIME_END");
            Property(x => x.TimeStart, "TimeStart").Column("TIME_START");
        }
    }
}
