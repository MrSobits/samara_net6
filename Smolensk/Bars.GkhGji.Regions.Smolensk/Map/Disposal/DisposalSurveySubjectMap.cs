/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Saha.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.GkhGji.Regions.Smolensk.Entities.Disposal;
/// 
///     public class DisposalSurveySubjectMap : BaseEntityMap<DisposalSurveySubject>
///     {
///         public DisposalSurveySubjectMap()
///             : base("GJI_DISP_SURVSUBJ")
///         {
///             this.References(x => x.SurveySubject, "SURVEY_SUBJ_ID", ReferenceMapConfig.NotNullAndFetch);
///             this.References(x => x.Disposal, "DISPOSAL_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Smolensk.Map.Disposal
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Smolensk.Entities.Disposal;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Smolensk.Entities.Disposal.DisposalSurveySubject"</summary>
    public class DisposalSurveySubjectMap : BaseEntityMap<DisposalSurveySubject>
    {
        
        public DisposalSurveySubjectMap() : 
                base("Bars.GkhGji.Regions.Smolensk.Entities.Disposal.DisposalSurveySubject", "GJI_DISP_SURVSUBJ")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Disposal, "Disposal").Column("DISPOSAL_ID").NotNull().Fetch();
            Reference(x => x.SurveySubject, "SurveySubject").Column("SURVEY_SUBJ_ID").NotNull().Fetch();
        }
    }
}
