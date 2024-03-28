/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг сущности "Цели проверки"
///     /// </summary>
///     public class TypeSurveyGoalInspGjiMap : BaseEntityMap<TypeSurveyGoalInspGji>
///     {
///         public TypeSurveyGoalInspGjiMap() : base("GJI_DICT_GOALS_INSPECTION")
///         {
///             Map(x => x.Name, "NAME").Length(2000);
///             Map(x => x.Code, "CODE").Length(100);
/// 
///             References(x => x.SurveyPurpose, "SURVEY_PURPOSE_ID").Not.Nullable().Fetch.Join();
///             References(x => x.TypeSurvey, "TYPE_SURVEY_GJI_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Цели проверки"</summary>
    public class TypeSurveyGoalInspGjiMap : BaseEntityMap<TypeSurveyGoalInspGji>
    {
        
        public TypeSurveyGoalInspGjiMap() : 
                base("Цели проверки", "GJI_DICT_GOALS_INSPECTION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Name, "Наименование").Column("NAME").Length(2000);
            Property(x => x.Code, "Код").Column("CODE").Length(100);
            Reference(x => x.SurveyPurpose, "Цели проверки").Column("SURVEY_PURPOSE_ID").NotNull().Fetch();
            Reference(x => x.TypeSurvey, "Тип обследования").Column("TYPE_SURVEY_GJI_ID").NotNull().Fetch();
        }
    }
}
