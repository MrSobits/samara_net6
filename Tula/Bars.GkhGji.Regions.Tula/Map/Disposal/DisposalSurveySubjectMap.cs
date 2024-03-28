/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Tula.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.GkhGji.Regions.Tula.Entities;
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

namespace Bars.GkhGji.Regions.Tula.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tula.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tula.Entities.DisposalSurveySubject"</summary>
    public class DisposalSurveySubjectMap : BaseEntityMap<DisposalSurveySubject>
    {
        
        public DisposalSurveySubjectMap() : 
                base("Bars.GkhGji.Regions.Tula.Entities.DisposalSurveySubject", "GJI_DISP_SURVSUBJ")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Disposal, "Disposal").Column("DISPOSAL_ID").NotNull().Fetch();
            Reference(x => x.SurveySubject, "SurveySubject").Column("SURVEY_SUBJ_ID").NotNull().Fetch();
        }
    }
}
