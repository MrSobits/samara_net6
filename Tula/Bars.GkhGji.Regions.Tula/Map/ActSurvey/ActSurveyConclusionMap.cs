/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Tula.Map
/// {
///     using Bars.B4.DataAccess;
/// 
///     using Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Заключение о тех состоянии акта обследования"
///     /// </summary>
///     public class ActSurveyConclusionMap : BaseEntityMap<ActSurveyConclusion>
///     {
///         public ActSurveyConclusionMap()
///             : base("GJI_ACTSURVEY_CONCLUSION")
///         {
///             Map(x => x.DocNumber, "DOC_NUMBER").Length(50);
///             Map(x => x.DocDate, "DOC_DATE");
///             Map(x => x.Description, "DESCRIPTION").Length(2000);
/// 
///             References(x => x.Official, "OFFICIAL_ID").Fetch.Join();
///             References(x => x.File, "FILE_ID").Fetch.Join();
///             References(x => x.ActSurvey, "ACT_SURVEY_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Tula.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tula.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tula.Entities.ActSurveyConclusion"</summary>
    public class ActSurveyConclusionMap : BaseEntityMap<ActSurveyConclusion>
    {
        
        public ActSurveyConclusionMap() : 
                base("Bars.GkhGji.Regions.Tula.Entities.ActSurveyConclusion", "GJI_ACTSURVEY_CONCLUSION")
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
