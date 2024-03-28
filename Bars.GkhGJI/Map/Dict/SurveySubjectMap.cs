/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.B4.DataAccess;
///     using GkhGji.Entities.Dict;
/// 
///     public class SurveySubjectMap : BaseEntityMap<SurveySubject>
///     {
///         public SurveySubjectMap()
///             : base("GJI_DICT_SURVEY_SUBJ")
///         {
///             this.Map(x => x.Name, "NAME").Length(500).Not.Nullable();
///             this.Map(x => x.Code, "CODE").Length(300);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities.Dict;
    
    /// <summary>Маппинг для "Справочники - ГЖИ - Предметы проверки"</summary>
    public class SurveySubjectMap : BaseEntityMap<SurveySubject>
    {
        
        public SurveySubjectMap() : 
                base("Справочники - ГЖИ - Предметы проверки", "GJI_DICT_SURVEY_SUBJ")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Name, "Name").Column("NAME").Length(2000).NotNull();
            Property(x => x.Code, "Code").Column("CODE").Length(300);
			Property(x => x.Formulation, "Formulation").Column("INSP_PLAN_FORMULATION").Length(500);
			Property(x => x.Relevance, "Relevance").Column("RELEVANCE").NotNull().DefaultValue(10);
            Property(x => x.GisGkhCode, "Код ГИС ЖКХ").Column("GIS_GKH_CODE");
            Property(x => x.GisGkhGuid, "ГИС ЖКХ GUID").Column("GIS_GKH_GUID").Length(36);
        }
    }
}
