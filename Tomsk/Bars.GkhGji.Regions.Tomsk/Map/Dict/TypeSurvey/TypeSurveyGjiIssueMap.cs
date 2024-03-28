/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Tomsk.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Regions.Tomsk.Entities;
/// 
///     /// <summary>
///     /// Маппинг сущности "По вопросу"
///     /// </summary>
///     public class TypeSurveyGjiIssueMap : BaseEntityMap<TypeSurveyGjiIssue>
///     {
///         public TypeSurveyGjiIssueMap(): base("GJI_TOMSK_TYPESURV_ISSUE")
///         {
///             this.Map(x => x.Name, "NAME").Not.Nullable().Length(300);
///             this.Map(x => x.Code, "CODE").Length(100);
/// 
///             this.References(x => x.TypeSurvey, "TYPE_SURVEY_GJI_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Tomsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tomsk.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tomsk.Entities.TypeSurveyGjiIssue"</summary>
    public class TypeSurveyGjiIssueMap : BaseEntityMap<TypeSurveyGjiIssue>
    {
        
        public TypeSurveyGjiIssueMap() : 
                base("Bars.GkhGji.Regions.Tomsk.Entities.TypeSurveyGjiIssue", "GJI_TOMSK_TYPESURV_ISSUE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Name, "Name").Column("NAME").Length(300).NotNull();
            Property(x => x.Code, "Code").Column("CODE").Length(100);
            Reference(x => x.TypeSurvey, "TypeSurvey").Column("TYPE_SURVEY_GJI_ID").NotNull().Fetch();
        }
    }
}
