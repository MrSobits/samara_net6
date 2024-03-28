/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map.Dict
/// {
///     using B4.DataAccess;
///     using Entities.Dict;
/// 
///     public class AuditPurposeSurveySubjectGjiMap : BaseEntityMap<AuditPurposeSurveySubjectGji>
///     {
///         public AuditPurposeSurveySubjectGjiMap()
///             : base("GJI_DICT_APURP_SSUBJ")
///         {
///             References(x => x.AuditPurpose, "AUDIT_PURP_ID");
///             References(x => x.SurveySubject, "SURV_SUBJ_ID");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities.Dict;
    
    
    /// <summary>Маппинг для "Справочники - ГЖИ - Цель проведения проверки"</summary>
    public class AuditPurposeSurveySubjectGjiMap : BaseEntityMap<AuditPurposeSurveySubjectGji>
    {
        
        public AuditPurposeSurveySubjectGjiMap() : 
                base("Справочники - ГЖИ - Цель проведения проверки", "GJI_DICT_APURP_SSUBJ")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.AuditPurpose, "Цель проведения проверки").Column("AUDIT_PURP_ID");
            Reference(x => x.SurveySubject, "Предметы проверки").Column("SURV_SUBJ_ID");
        }
    }
}
