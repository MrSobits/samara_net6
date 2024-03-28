/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг сущности "Задачи проверки"
///     /// </summary>
///     public class TypeSurveyTaskInspGjiMap : BaseEntityMap<TypeSurveyTaskInspGji>
///     {
///         public TypeSurveyTaskInspGjiMap()
///             : base("GJI_DICT_TASKS_INSPECTION")
///         {
///             Map(x => x.Name, "NAME").Length(2000);
///             Map(x => x.Code, "CODE").Length(100);
/// 
///             References(x => x.SurveyObjective, "SURVEY_OBJECTIVE_ID").Not.Nullable().Fetch.Join();
///             References(x => x.TypeSurvey, "TYPE_SURVEY_GJI_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Задачи проверки"</summary>
    public class TypeSurveyTaskInspGjiMap : BaseEntityMap<TypeSurveyTaskInspGji>
    {
        
        public TypeSurveyTaskInspGjiMap() : 
                base("Задачи проверки", "GJI_DICT_TASKS_INSPECTION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Name, "Наименование").Column("NAME").Length(2000);
            Property(x => x.Code, "Код").Column("CODE").Length(100);
            Reference(x => x.SurveyObjective, "Задача проверки").Column("SURVEY_OBJECTIVE_ID").NotNull().Fetch();
            Reference(x => x.TypeSurvey, "Тип обследования").Column("TYPE_SURVEY_GJI_ID").NotNull().Fetch();
        }
    }
}
