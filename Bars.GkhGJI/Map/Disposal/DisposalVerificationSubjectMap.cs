/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Chelyabinsk.Map.Disposal
/// {
///     using B4.DataAccess.ByCode;
///     using Entities.Disposal;
/// 
///     public class DisposalVerificationSubjectMap : BaseEntityMap<DisposalVerificationSubject>
///     {
///         public DisposalVerificationSubjectMap()
///             : base("GJI_NSO_DISP_VERIFSUBJ")
///         {
///             Map(x => x.TypeVerificationSubject, "TYPE_VERIF_SUBJ");
/// 
///             References(x => x.Disposal, "DISPOSAL_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.SurveySubject, "SURVEY_SUBJECT_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Chelyabinsk.Entities.Disposal.DisposalVerificationSubject"</summary>
    public class DisposalVerificationSubjectMap : BaseEntityMap<DisposalVerificationSubject>
    {
        
        public DisposalVerificationSubjectMap() : 
                base("Bars.GkhGji.Entities.DisposalVerificationSubject", "GJI_NSO_DISP_VERIFSUBJ")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.Disposal, "Disposal").Column("DISPOSAL_ID").NotNull().Fetch();
            this.Reference(x => x.SurveySubject, "SurveySubject").Column("SURVEY_SUBJECT_ID").Fetch();
        }
    }
}
